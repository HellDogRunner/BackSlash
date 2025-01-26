using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	public class MovementController : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;

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
		private bool _isSprint;
		private bool _inAir;
		private bool _inJump;
		private bool _canJump;
		private bool _canFall;
		private bool _canSprint;

		private float _requiredSpeed;
		private float _ySpeed;

		private Vector3 _airDirection, _moveDirection;

		private Transform _camera;
		private TargetLock _targetLock;
		private InputController _inputController;

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
		private void Construct(TargetLock targetLock, InputController inputController)
		{
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

			_requiredSpeed = _isSprint ? 2 : 1;
			if (!_tryMove) _requiredSpeed = 0;

			OnFreeMove?.Invoke(_requiredSpeed);
			OnLockMove?.Invoke(_inputController.MoveDirection);	

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
			if (!_inAir && !_inJump && _canJump)
			{
				_inJump = true;
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
			if (_canSprint && !_inAir)
			{
				_isSprint = pressed ? true : false;
				OnSprint?.Invoke(pressed);
			}
		}

		public bool CanLockedRotate()
		{
			return _isSprint && !_inAir;
		}
		
		public void SetCanJump(bool value)
		{
			_canJump = value;
		}
		
		public void SetCanFall(bool value)
		{
			_canFall = value;
		}
		
		public void SetCanSprint(bool value)
		{
			_canSprint = value;
			if (!value)
			{
				OnSprint?.Invoke(false);
				_isSprint = false;	
			}
		}

		private void CheckLand()
		{
			if (!IsGrounded() && !_inAir)
			{
				_airDirection = _targetLock.Target ? GetMoveDirection() : GetJumpDirection();
				_inAir = true;
				InAir?.Invoke(true);
				
				if (!_inJump)
				{
					_ySpeed = 0;
					if (_canFall) OnFall?.Invoke();
				}
			}

			if (IsGrounded() && _inAir)
			{
				_inAir = false;
				InAir?.Invoke(false);
				if (Physics.gravity.y > _ySpeed)
				{
					OnLanding?.Invoke();
					_ySpeed = Physics.gravity.y;
				}
				StartCoroutine(JumpDelay());
			}
		}

		private IEnumerator JumpDelay()
		{
			_inJump = true;
			yield return new WaitForSeconds(_jumpDelay);
			_inJump = false;
		}

		private void InvokeSteps()
		{
			PlaySteps?.Invoke(!_inAir && _canFall);
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
