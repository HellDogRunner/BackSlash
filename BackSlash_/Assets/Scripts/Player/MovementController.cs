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
		[SerializeField] private Vector2 _movePlayer;
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
		
		[Header("SlopeAngle")]
		[SerializeField] private float _maxSlopeAngle;
		
		[Header("IsGround settings")]
		[SerializeField] private float _maxCastDistance;
		[SerializeField] private float _sphereCastRadius;
		[SerializeField] private LayerMask _hitboxLayer;

		private enum EMove
		{ None, Dodge, Attack, Block }

		private EMove _move;

		private bool _tryMove;
		private bool _inAir;
		private bool _isJump;
		private bool _isSprint;
		private bool _isLock;
		private bool _canJump = true;
		private bool _canDodge = true;

		private float _requiredSpeed;
		private float _yForce;

		private Vector3 _lockedDirection;
		[SerializeField] private Vector2 _offsetDirection;

		private InputController _inputController;
		private ComboSystem _comboSystem;
		private TargetLock _targetLock;
		private Transform _camera;

		public bool IsSprint => _isSprint;
		// public float Speed => _currentSpeed;

		public event Action<Vector2> OnLockMove;
		public event Action<float> OnFreeMove;
		public event Action<bool> OnTryMove;
		public event Action<bool> PlaySteps;
		public event Action<bool> OnSprint;
		public event Action<bool> OnBlock;
		public event Action<bool> InAir;
		public event Action OnDodge;
		public event Action OnJump;

		//debug gizmo parameter delete later
		private float _currenthitdisance;

		[Inject]
		private void Construct(InputController inputController, ComboSystem comboSystem, TargetLock targetLock)
		{
			_inputController = inputController;
			_comboSystem = comboSystem;
			_targetLock = targetLock;
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
			_targetLock.OnSwitchLock += Lock;
		}

		private void OnDisable()
		{
			_inputController.OnDirectionChanged -= Move;
			_inputController.OnSprintKeyPressed -= Sprint;
			_inputController.OnJumpKeyPressed -= Jump;
			_inputController.OnDodgeKeyPressed -= Dodge;
			_inputController.OnBlockPressed -= Block;
			_comboSystem.IsAttacking -= IsAttacking;
			_targetLock.OnSwitchLock -= Lock;
		}

		private void Update()
		{
			CheckLand();
			MovePlayer();
			InvokeSteps();
		}
		
		private void Move() 
		{
			var direction = _inputController.MoveDirection;

			if (direction == Vector2.zero)
			{
				_tryMove = false;
				_requiredSpeed = 0;
			}
			else
			{
				_tryMove = true;
				_requiredSpeed = _runSpeed;
				if (_inAir) _requiredSpeed = _airSpeed;
				if (_isLock)
				{
					if (direction.x != 0 || direction.y == -1) _requiredSpeed = _lockedSpeed;
				}
				if (_isSprint) _requiredSpeed = _sprintSpeed;
			}
			
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
			else
			{
				if (!_tryMove && _currentSpeed != _requiredSpeed || _move == EMove.Attack)
				{
					//_moveDirection /= _currentSpeed;
					//ChangeForce(_standMulti);
					//direction = _moveDirection * _currentSpeed;
				}
				else
				{
					//ChangeForce(_moveMulti);
					//direction = GetMoveDirection() * _currentSpeed;
				}
			}
			
			if (_move == EMove.Dodge) //direction = _lockedDirection; блочить направление при додже
			
			direction.y = _yForce;
			_moveDirection = direction;
			_characterController.Move(_moveDirection * Time.deltaTime);
		}
		
		private void MoveState(EMove move = EMove.None)
		{
			switch (move)
			{
				case EMove.None:
					_requiredSpeed = _runSpeed;
					break;
				
				case EMove.Attack:
					_requiredSpeed = 0;
					break;
				
				case EMove.Dodge:
					_lockedDirection = GetMoveDirection() * _runSpeed;
					_lockedDirection.y = _yForce;
					break;
				
				case EMove.Block:
					_requiredSpeed = _walkSpeed;
					break;
			}
			
			_move = move;
			SendEvents();
		}
		
		private void SendEvents()
		{
			if (_move != EMove.Block || _inAir) OnBlock?.Invoke(false);
			if (_move == EMove.Block) OnBlock?.Invoke(true);
			OnSprint?.Invoke(false);
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
			if (!_inAir && _canJump && _move != EMove.Dodge && _move != EMove.Attack)
			{
				_canJump = false;
				_isJump = true;
				OnJump?.Invoke();
				_yForce = _jumpSpeed;
			}
		}

		// TODO Evasion animation
		// If the player does not move then play an evasion animation
		// When the player moves, play a dodge animation
		
		// Speed up the animatio
		
		// Separate evasion animations when locked on enemy
		// * forward			| default forward roll
		// * left, right, back	| a small leap in the direction
		
		// FIXME Rotate player when spam dodge
		// Roll may have the wrong direction
		private void Dodge()
		{
			if (!_inAir && _canDodge && _move != EMove.Attack)
			{
				_canDodge = false;
				OnDodge?.Invoke();
				MoveState(EMove.Dodge);
				StartCoroutine(DodgeDelay());
			}
		}

		private void Sprint(bool pressed)
		{
			_isSprint = pressed ? true : false;
			OnSprint?.Invoke(pressed);
			Move();
		}
		
		// TODO Realize block
		// TODO Special animation for movement when block
		private void Block(bool pressed)
		{
			if (pressed && _move == EMove.None) MoveState(EMove.Block);
			if (!pressed && _move == EMove.Block) MoveState();
		}

		// TODO Start moving before the attack ends
		// take the animation time from combo system ??
		private void IsAttacking(bool isAttacking)
		{
			if (isAttacking && _move != EMove.Dodge) MoveState(EMove.Attack);
			if (!isAttacking) MoveState();
		}

		private void Lock(bool locked)
		{
			_isLock = locked ? true : false;
			Move();
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
				SendEvents();
			}

			if (IsGrounded() && _inAir)
			{
				_inAir = false;
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

		// TODO Invoke after dodge or get the rolling animation time
		private IEnumerator DodgeDelay()
		{
			yield return new WaitForSeconds(_dodgeCooldown + _dodgeDelay);
			MoveState();
			_canDodge = true;
		}

		private void InvokeSteps()
		{
			if (_tryMove && !_inAir && _move != EMove.Attack && _move != EMove.Dodge)
			{
				PlaySteps?.Invoke(true);
			}
			else
			{
				PlaySteps?.Invoke(false);
			}
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

		private bool OnSlope()
		{
			if (Physics.Raycast(transform.position, Vector3.down, out var _slopeHit, _characterController.height * 0.5f + 1.5f))
			{
				float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
				if (angle > _maxSlopeAngle)
				{
					return true;
				}
			}
			return false;
		}
		
		public bool CanAttack()
		{
			if (_move == EMove.Dodge) return false;
			return true;
		}

		public bool CanRotate()
		{
			if (_move == EMove.Dodge || _inAir) return false;
			return true;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Debug.DrawLine(gameObject.transform.position + _characterController.center + (Vector3.up * 0.1f), gameObject.transform.position + (_characterController.center + (Vector3.up * 0.1f)) + Vector3.down * _currenthitdisance, Color.yellow);
			Gizmos.DrawWireSphere(gameObject.transform.position + (_characterController.center + (Vector3.up * 0.1f)) + Vector3.down * _currenthitdisance, _sphereCastRadius);
		}
	}
}
