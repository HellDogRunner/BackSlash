using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	public class MovementController : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;

		[Header("Monitoring")]
		[SerializeField] private Vector3 _moveDirection;

		[Header("Settings")]
		[SerializeField] private float _jumpSpeed;
		[SerializeField] private float _jumpDelay;
		[Space]
		[SerializeField] private float _airSpeed;
		[SerializeField] private float _airDirectionMulti;
		[SerializeField] private float _gravityMulti;
		[SerializeField] private float _yMaxSpeed;

		[Header("IsGround settings")]
		[SerializeField] private float _maxCastDistance;
		[SerializeField] private float _sphereCastRadius;
		[SerializeField] private LayerMask _hitboxLayer;

		private bool _tryMove;
		private bool _trySprint;
		private bool _inAir;
		private bool _canJump = true;

		private float _requiredSpeed;
		private float _ySpeed;

		private Vector3 _airDirection;

		private Transform _camera;
		private TargetLock _targetLock;
		private InputController _inputController;
		private PlayerStateController _stateController;

		public bool TrySprint => _trySprint;
		public bool Air => _inAir;

		public event Action<Vector2> OnLockMove;
		public event Action<float> OnFreeMove;
		public event Action<bool> OnTryMove;
		public event Action<bool> PlaySteps;
		public event Action<bool> OnSprint;
		public event Action<bool> InAir;
		public event Action OnLanding;
		public event Action OnFall;
		public event Action OnJump;

		//debug gizmo parameter delete later
		private float _currenthitdisance;

		[Inject]
		private void Construct(PlayerStateController playerStateController, TargetLock targetLock, InputController inputController)
		{
			_stateController = playerStateController;
			_inputController = inputController;
			_targetLock = targetLock;
		}

		private void Awake()
		{
			_camera = Camera.main.transform;
		}

		private void OnEnable()
		{
			_inputController.OnSprintKeyPressed += Sprint;
			_inputController.OnDirectionChanged += Move;
			_inputController.OnJumpKeyPressed += Jump;
		}

		private void OnDisable()
		{
			_inputController.OnSprintKeyPressed -= Sprint;
			_inputController.OnDirectionChanged -= Move;
			_inputController.OnJumpKeyPressed -= Jump;
		}

		private void Update()
		{
			CheckLand();
			MovePlayer();
			InvokeSteps();
		}

		private void Move()
		{
			_tryMove = _inputController.MoveDirection != Vector2.zero;
			OnTryMove?.Invoke(_tryMove);
		}

		private void MovePlayer()
		{
			var direction = Vector3.zero;

			_requiredSpeed = _trySprint ? 2 : 1;
			if (!_tryMove) _requiredSpeed = 0;
			if (_stateController.CanMove())
			{
				OnFreeMove?.Invoke(_requiredSpeed);
				OnLockMove?.Invoke(_inputController.MoveDirection);	
			}

			if (_inAir)
			{
				_ySpeed = Mathf.Lerp(_ySpeed, _yMaxSpeed, Time.deltaTime * _gravityMulti);
				_airDirection = TryNormalize(_airDirection + GetMoveDirection() * _airDirectionMulti);
				direction = _airDirection * _airSpeed;
			}
			else if (_ySpeed > Physics.gravity.y) _ySpeed = Mathf.Lerp(_ySpeed, _yMaxSpeed, Time.deltaTime * _gravityMulti);

			direction.y = _ySpeed;
			_moveDirection = direction;
			_characterController.Move(_moveDirection * Time.deltaTime);
		}

		private void Jump()
		{
			if (!_inAir && _canJump && _stateController.CanJump())
			{
				_canJump = false;
				OnJump?.Invoke();
			}
		}
		
		private void JumpStart()
		{
			_ySpeed = _jumpSpeed;
		}
		
		private void JumpEnd()
		{
			if (_inAir)
			{
				OnFall?.Invoke();
			}
		}

		private void Sprint(bool pressed)
		{
			_trySprint = pressed ? true : false;
			OnSprint?.Invoke(pressed);
		}

		private void CheckLand()
		{
			if (!IsGrounded() && !_inAir)
			{
				_airDirection = _targetLock.Target ? GetMoveDirection() : GetJumpDirection();
				_inAir = true;
				InAir?.Invoke(true);
				
				if (_canJump)
				{
					_ySpeed = 0;
					if (_stateController.State == EPlayerState.None) OnFall?.Invoke();
				}
			}

			if (IsGrounded() && _inAir)
			{
				_inAir = false;
				InAir?.Invoke(false);
				if (Physics.gravity.y >= _ySpeed)
				{
					OnLanding?.Invoke();
					_ySpeed = Physics.gravity.y;
				}
				StartCoroutine(JumpDelay());
			}
		}

		private IEnumerator JumpDelay()
		{
			_canJump = false;
			yield return new WaitForSeconds(_jumpDelay);
			_canJump = true;
		}

		private void InvokeSteps()
		{
			PlaySteps?.Invoke(!_inAir && _stateController.State == EPlayerState.None);
		}

		private Vector3 TryNormalize(Vector3 direction)
		{
			if (Math.Abs(direction.x) > 1 || Math.Abs(direction.z) > 1) return direction.normalized;
			return direction;
		}

		private Vector3 GetMoveDirection()
		{
			var direction = _inputController.MoveDirection;
			return (direction.y * _camera.forward + direction.x * _camera.right).normalized;
		}

		private Vector3 GetJumpDirection()
		{
			return _inputController.MoveDirection == Vector2.zero ? Vector2.zero : transform.forward.normalized;
		}

		private bool IsGrounded()
		{
			if (Physics.SphereCast(
				gameObject.transform.position + _characterController.center + (Vector3.up * 0.1f),
				_sphereCastRadius,
				Vector3.down, out var hitInfo,
				_maxCastDistance,
				_hitboxLayer,
				QueryTriggerInteraction.Ignore))
			{
				_currenthitdisance = hitInfo.distance;
				return true;
			}
			else
			{
				_currenthitdisance = _maxCastDistance;
				return false;
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Debug.DrawLine(gameObject.transform.position + _characterController.center + (Vector3.up * 0.1f), gameObject.transform.position + (_characterController.center + (Vector3.up * 0.1f)) + Vector3.down * _currenthitdisance, Color.yellow);
			Gizmos.DrawWireSphere(gameObject.transform.position + (_characterController.center + (Vector3.up * 0.1f)) + Vector3.down * _currenthitdisance, _sphereCastRadius);
		}
	}
}
