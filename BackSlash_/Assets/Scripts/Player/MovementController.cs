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
		[SerializeField] private float _currentSpeed;
		[SerializeField] private Vector3 _moveDirection;
		
		[Header("Settings")]
		[SerializeField] private float _walkSpeed;
		[SerializeField] private float _lockedSpeed;
		[SerializeField] private float _runSpeed;
		[SerializeField] private float _sprintSpeed;
		[SerializeField] private float _moveMulti;
		[SerializeField] private float _lockedMulti;
		[Space]
		[SerializeField] private float _jumpSpeed;
		[SerializeField] private float _jumpDelay;
		[Space]
		[SerializeField] private float _dodgeCooldown;
		[SerializeField] private float _dodgeDelay;
		[Space]
		[SerializeField] private float _airSpeed;
		[SerializeField] private float _airDirectionMulti;
		[SerializeField] private float _gravityMulti;
		
		[Header("IsGround settings")]
		[SerializeField] private float _maxCastDistance;
		[SerializeField] private float _sphereCastRadius;
		[SerializeField] private LayerMask _hitboxLayer;

		private bool _tryMove;
		private bool _inAir;
		private bool _isJump;
		private bool _isSprint;
		private bool _canJump = true;
		private bool _canDodge = true;

		private float _requiredSpeed;
		private float _yForce;

		private Vector3 _lockedDirection;
		[SerializeField] private Vector2 _offsetDirection;

		private InputController _inputController;
		private PlayerStateController _state;
		private ComboSystem _comboSystem;
		private Transform _camera;

		public bool IsSprint => _isSprint;
		public bool Air => _inAir;
		// public float Speed => _currentSpeed;

		public event Action<Vector2> OnLockMove;
		public event Action<float> OnFreeMove;
		public event Action<bool> OnTryMove;
		public event Action<bool> PlaySteps;
		public event Action<bool> OnSprint;
		public event Action<bool> InAir;
		public event Action OnJump;

		//debug gizmo parameter delete later
		private float _currenthitdisance;

		[Inject]
		private void Construct(PlayerStateController stateController, InputController inputController, ComboSystem comboSystem)
		{
			_inputController = inputController;
			_state = stateController;
			_comboSystem = comboSystem;
		}

		private void Awake()
		{
			_camera = Camera.main.transform;
			_requiredSpeed = _runSpeed;
		}

		private void OnEnable()
		{
			_inputController.OnDirectionChanged += Move;
			_inputController.OnSprintKeyPressed += Sprint;
			_inputController.OnJumpKeyPressed += Jump;
			_inputController.OnDodgeKeyPressed += Dodge;
			_inputController.OnBlockPressed += Block;
			_comboSystem.IsAttacking += IsAttacking;
		}

		private void OnDisable()
		{
			_inputController.OnDirectionChanged -= Move;
			_inputController.OnSprintKeyPressed -= Sprint;
			_inputController.OnJumpKeyPressed -= Jump;
			_inputController.OnDodgeKeyPressed -= Dodge;
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
			_tryMove = _inputController.MoveDirection != Vector2.zero ? true : false;
			OnTryMove?.Invoke(_tryMove);
		}

		private void MovePlayer()
		{	
			var direction = Vector3.zero;
			
			_requiredSpeed = _isSprint ? 2 : 1;
			if (!_tryMove) _requiredSpeed = 0;
			
			OnLockMove?.Invoke(_inputController.MoveDirection);
			OnFreeMove?.Invoke(_requiredSpeed);

			if (_inAir)
			{
				_yForce += Physics.gravity.y * Time.deltaTime * _gravityMulti;
				_lockedDirection = TryNormalize(_lockedDirection + GetMoveDirection() * _airDirectionMulti);
				direction = _lockedDirection * _currentSpeed;
			}
			// FIXME игрок после прыжка не приземляется
			// баг с левитацией
			// починить доджи и остальныйе стейты
			// интеракшн и пайза 100 проц сломались
			if (_state.State == EPlayerState.Dodge) //direction = _lockedDirection; блочить направление при додже
			
			direction.y = _yForce;
			_moveDirection = direction;
			_characterController.Move(_moveDirection * Time.deltaTime);
		}

		private void ChangeSpeed(float multi)
		{
			float coef;
			if (_currentSpeed == _requiredSpeed) return;
			else coef = _currentSpeed < _requiredSpeed ? 1 : -1;
			
			coef = coef * Time.deltaTime * multi;
			_currentSpeed += coef;
			
			if (Math.Abs(_requiredSpeed - _currentSpeed) <= coef) _currentSpeed = _requiredSpeed;
		}
		
		private void Jump()
		{
			if (!_inAir && _canJump && _state.CanJump())
			{
				_canJump = false;
				_isJump = true;
				OnJump?.Invoke();
				_yForce = _jumpSpeed;
			}
		}

		private void Dodge()
		{
			if (!_inAir && _canDodge && _state.CanDodge())
			{
				_canDodge = false;
				_state.Dodge();
				StartCoroutine(DodgeDelay());
			}
		}

		private void Sprint(bool pressed)
		{
			_isSprint = pressed ? true : false;
			OnSprint?.Invoke(pressed);
		}
		
		// TODO Realize block
		// TODO Special animation for movement when block
		private void Block(bool pressed)
		{
			if (pressed) _state.Block();
			else _state.None();
		}

		// TODO Start moving before the attack ends
		// take the animation time from combo system ??
		private void IsAttacking(bool isAttacking)
		{
			if (isAttacking) _state.Attack();
			else _state.None();
		}

		private void CheckLand()
		{
			if (!IsGrounded() && !_inAir)
			{
				_currentSpeed = _airSpeed;
				_lockedDirection = GetMoveDirection();
				_inAir = true;
				InAir?.Invoke(true);
				if (_isJump) _isJump = false;		
				else _yForce = 0;
			}

			if (IsGrounded() && _inAir)
			{
				_inAir = false;
				InAir?.Invoke(false);
				_yForce = Physics.gravity.y;
				Debug.Log("grounded");
				StartCoroutine(JumpDelay());
			}
		}

		private IEnumerator JumpDelay()
		{
			yield return new WaitForSeconds(_jumpDelay);
			Debug.Log("can jump delay");
			_canJump = true;
		}

		// TODO Invoke after dodge or get the rolling animation time
		private IEnumerator DodgeDelay()
		{
			yield return new WaitForSeconds(_dodgeCooldown + _dodgeDelay);
			_canDodge = true;
		}

		private void InvokeSteps()
		{
			PlaySteps?.Invoke(!_inAir && _state.State == EPlayerState.None);
		}

		private Vector3 TryNormalize(Vector3 dir)
		{
			if (Math.Abs(dir.x) > 1 || Math.Abs(dir.z) > 1) return dir.normalized;
			return dir;
		}

		public Vector3 GetMoveDirection()
		{
			var direction = _inputController.MoveDirection;
			return (direction.y * _camera.forward + direction.x * _camera.right).normalized;
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
