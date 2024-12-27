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
		private bool _isJump;
		private bool _inAir;
		private bool _isFall;
		private bool _animateFall;
		private bool _canJump = true;
		private bool _canDodge = true;

		private float _requiredSpeed;
		private float _yForce;

		private Vector3 _lockedDirection;

		private Transform _camera;
		private TargetLock _targetLock;
		private ComboSystem _comboSystem;
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
		public event Action OnFalling;
		public event Action OnDodge;
		public event Action OnFall;
		public event Action OnJump;

		//debug gizmo parameter delete later
		private float _currenthitdisance;

		[Inject]
		private void Construct(TargetLock targetLock, PlayerStateController stateController, InputController inputController, ComboSystem comboSystem)
		{
			_inputController = inputController;
			_stateController = stateController;
			_comboSystem = comboSystem;
			_targetLock = targetLock;
		}

		private void Awake()
		{
			_camera = Camera.main.transform;
		}

		private void OnEnable()
		{
			_inputController.OnSprintKeyPressed += Sprint;
			_inputController.OnDodgeKeyPressed += Dodge;
			_inputController.OnDirectionChanged += Move;
			_inputController.OnJumpKeyPressed += Jump;
			_inputController.OnBlockPressed += Block;
			_comboSystem.IsAttacking += IsAttacking;
		}

		private void OnDisable()
		{
			_inputController.OnSprintKeyPressed -= Sprint;
			_inputController.OnDodgeKeyPressed -= Dodge;
			_inputController.OnDirectionChanged -= Move;
			_inputController.OnJumpKeyPressed -= Jump;
			_inputController.OnBlockPressed -= Block;
			_comboSystem.IsAttacking -= IsAttacking;
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

			OnFreeMove?.Invoke(_requiredSpeed);
			OnLockMove?.Invoke(_inputController.MoveDirection);
			if (!_canDodge) OnDodge?.Invoke();

			if (_inAir)
			{
				if (_yForce <= Physics.gravity.y && !_isFall) _isFall = true;

				_yForce = Mathf.Lerp(_yForce, _yMaxSpeed, Time.deltaTime * _gravityMulti);
				_lockedDirection = TryNormalize(_lockedDirection + GetMoveDirection() * _airDirectionMulti);
				direction = _lockedDirection * _airSpeed;
			}
			
			if (_animateFall) OnFalling?.Invoke();

			direction.y = _yForce;
			_moveDirection = direction;
			_characterController.Move(_moveDirection * Time.deltaTime);
		}

		private void Jump()
		{
			if (!_inAir && _canJump && _stateController.CanJump())
			{
				_canJump = false;
				_isJump = true;
				OnJump?.Invoke();
				_yForce = _jumpSpeed;
			}
		}
		
		private void JumpEnd()
		{
			if (_inAir)
			{
				OnFall?.Invoke();
				_animateFall = true;
			}
		}
		
		private void EndAnimationEvent()
		{ 
			if (_canDodge)
			{
				//Debug.Log("end dodge");
				_stateController.SetNone(); 
			}
		}
		private void DodgeAnimationEvent(int value) { _canDodge = value == 1; }

		private void Dodge()
		{
			if (!_inAir && _canDodge)
			{
				_stateController.SetDodge();
			}
		}

		private void Sprint(bool pressed)
		{
			_trySprint = pressed ? true : false;
			OnSprint?.Invoke(pressed);
		}

		// TODO Realize block
		private void Block(bool pressed)
		{
			if (pressed) _stateController.SetBlock();
			else if (_stateController.State == EPlayerState.Block) _stateController.SetNone();
		}

		// TODO Start moving before the attack ends?
		// take the animation time from combo system?
		private void IsAttacking(bool isAttacking)
		{
			if (isAttacking) _stateController.SetAttack();
			else _stateController.SetNone();
		}

		private void CheckLand()
		{
			if (!IsGrounded() && !_inAir)
			{
				_lockedDirection = _targetLock.Target ? GetMoveDirection() : GetJumpDirection();
				_inAir = true;
				InAir?.Invoke(true);
				if (_isJump) _isJump = false;
				else
				{
					_yForce = 0;
					OnFall?.Invoke();
					_animateFall = true;
				}
			}

			if (IsGrounded() && _inAir)
			{
				_inAir = false;
				_animateFall = false;
				if (_isFall)
				{
					OnLanding?.Invoke();
					_isFall = false;
				}
				InAir?.Invoke(false);
				_yForce = Physics.gravity.y;
				StartCoroutine(JumpDelay());
			}
		}

		private IEnumerator JumpDelay()
		{
			yield return new WaitForSeconds(_jumpDelay);
			_canJump = true;
		}

		private void InvokeSteps()
		{
			PlaySteps?.Invoke(!_inAir && _stateController.State == EPlayerState.None);
		}

		private Vector3 TryNormalize(Vector3 dir)
		{
			if (Math.Abs(dir.x) > 1 || Math.Abs(dir.z) > 1) return dir.normalized;
			return dir;
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
