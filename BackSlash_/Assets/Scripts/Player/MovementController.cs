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
		[Header("Movement settings")]
		[SerializeField] private float _walkSpeed;
		[SerializeField] private float _runSpeed;
		[SerializeField] private float _sprintSpeed;
		[SerializeField] private float _moveMulti;
		[SerializeField] private float _standMulti;
		[SerializeField] private float _attackMulti;
		[SerializeField] private float _jumpForce;
		[SerializeField] private float _jumpDelay;
		[SerializeField] private float _airSpeedMulti;
		[SerializeField] private float _airDirectionMulti;
		[SerializeField] private float _gravityMulti;
		[SerializeField] private float _dodgeCooldown;
		[SerializeField] private float _dodgeDelay;
		[Header("SlopeAngle")]
		[SerializeField] private float _maxSlopeAngle;
		[Header("IsGround settings")]
		[SerializeField] private float _maxCastDistance;
		[SerializeField] private float _sphereCastRadius;
		[SerializeField] private LayerMask _hitboxLayer;

		private enum EMove
		{ None, Stand, Move, Jump, Fall, Dodge, Attack, Block }

		[SerializeField] private EMove _move;

		private bool _isLanded;
		private bool _canJump = true;
		private bool _canDodge = true;

		private float _requiredSpeed;
		private float _ySpeed;

		private Vector3 _forwardDirection;
		private Vector3 _lockedDirection;

		private InputController _inputController;
		private ComboSystem _comboSystem;
		private Transform _camera;
		private GameControls _controls;

		public event Action<Vector2> OnMoving;
		public event Action<bool> PlaySteps;
		public event Action<bool> OnSprint;
		public event Action<bool> OnBlock;
		public event Action<bool> InAir;
		public event Action OnDodge;
		public event Action OnJump;

		//debug gizmo parameter delete later
		private float _currenthitdisance;

		[Inject]
		private void Construct(InputController inputController, ComboSystem comboSystem)
		{
			_inputController = inputController;
			_comboSystem = comboSystem;
		}

		private void Awake()
		{
			_camera = Camera.main.transform;
			_controls = _inputController.Controls;
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
			var direction = _inputController.MoveDirection;
			
			if (_move == EMove.Jump || _move == EMove.Fall)
			{
				if (direction == Vector2.zero) _requiredSpeed = 0;
				else _requiredSpeed = _runSpeed;
			}
			
			if (_move != EMove.Dodge && _move != EMove.Block && _move != EMove.Attack) ChangeMoveState();
		}

		private void MovePlayer()
		{	
			OnMoving?.Invoke(_inputController.MoveDirection);
			
			if (!IsGrounded()) _ySpeed += Physics.gravity.y * Time.deltaTime * _gravityMulti;
			
			if (_move == EMove.Dodge) 
			{
				_lockedDirection.y = _ySpeed;
				_characterController.Move(_lockedDirection * Time.deltaTime);
				return;
			}
			// FIXME при атаке в воздухе игрок навсегда зависает
			if (_move == EMove.Attack)
			{
				ChangeSpeed(_attackMulti);
				return;
			}
			if (_move == EMove.Stand && _currentSpeed != _requiredSpeed)
			{
				ChangeSpeed(_standMulti); 
				_moveDirection = _forwardDirection.normalized * _currentSpeed;
				_moveDirection.y = _ySpeed;
				_characterController.Move(_moveDirection * Time.deltaTime);
				return;
			}
			if (_move == EMove.Jump || _move == EMove.Fall)
			{
				ChangeSpeed(_airSpeedMulti); 
				_moveDirection = GetForwardDirection();
				var direction = _lockedDirection + _moveDirection * _airDirectionMulti * _currentSpeed;
				direction.y = _ySpeed;
				_characterController.Move(direction * Time.deltaTime);
				return;
			}
			
			if (_move == EMove.Move || _move == EMove.Block) ChangeSpeed(_moveMulti);
			
			_moveDirection = GetForwardDirection();
			_moveDirection *= _currentSpeed;
			_moveDirection.y = _ySpeed;

			_characterController.Move(_moveDirection * Time.deltaTime);
		}
		
		private bool ChangeMoveState(EMove move = EMove.None)
		{
			var direction = _inputController.MoveDirection;
			
			if (move == EMove.None && direction == Vector2.zero && _isLanded)
			{
				_requiredSpeed = 0;
				_move = EMove.Stand;
				SendEvents();
				return true;
			}
			
			if (move == EMove.None && direction != Vector2.zero && _isLanded)
			{
				_requiredSpeed = _runSpeed;
				_move = EMove.Move;
				SendEvents();
				return true;
			}
			
			if ((move == EMove.Jump || move == EMove.Fall) && _move != EMove.Jump && _move != EMove.Dodge && _move != EMove.Attack)
			{
				_lockedDirection = GetForwardDirection();
				_lockedDirection *= _currentSpeed;
				_requiredSpeed = _runSpeed;
				_ySpeed = move == EMove.Jump ? _jumpForce : 0;
				_canJump = false;
				_move = move;
				SendEvents();
				return true;
			}
			
			if (move == EMove.Dodge && _move != EMove.Attack && _move != EMove.Jump && _move != EMove.Fall)
			{
				_lockedDirection = GetForwardDirection();
				_lockedDirection *= _runSpeed;
				_move = move;
				SendEvents();
				return true;
			}
			
			if (move == EMove.Attack && _move != EMove.Dodge)
			{
				_requiredSpeed = 0;
				_move = move;
				SendEvents();
				return true;
			}
			
			if (move == EMove.Block && _move != EMove.Dodge && _move != EMove.Attack)
			{
				_requiredSpeed =  _runSpeed;
				_move = move;
				SendEvents();
				return true;
			}
			
			return false;
		}
		
		private void SendEvents()
		{
			if (_move == EMove.Stand)
			{
				InAir?.Invoke(false);
				OnSprint?.Invoke(false);
				OnBlock?.Invoke(false);
			}
			if (_move == EMove.Move)
			{
				InAir?.Invoke(false);
				OnBlock?.Invoke(false);
			}
			if (_move == EMove.Jump)
			{
				OnSprint?.Invoke(false);
				OnBlock?.Invoke(false);
				InAir?.Invoke(true);
				OnJump?.Invoke();
			}
			if (_move == EMove.Fall)
			{
				OnSprint?.Invoke(false);
				OnBlock?.Invoke(false);
				InAir?.Invoke(true);
			}
			if (_move == EMove.Dodge) 
			{
				OnSprint?.Invoke(false);
				OnBlock?.Invoke(false);
				OnDodge?.Invoke();
			}
			if (_move == EMove.Attack)
			{
				OnSprint?.Invoke(false);
				OnBlock?.Invoke(false);
			}
			if (_move == EMove.Block)
			{
				OnSprint?.Invoke(false);
				OnBlock?.Invoke(true);
			}
		}
		
		private void ChangeSpeed(float multi)
		{
			float coef;
			if (_currentSpeed == _requiredSpeed) return;
			else if (_currentSpeed < _requiredSpeed) coef = 1;
			else coef = -1;
			
			_currentSpeed += coef * Time.deltaTime * 10 * multi;
			
			if (Math.Abs(_requiredSpeed - _currentSpeed) < 0.5f) _currentSpeed = _requiredSpeed;
		}
		
		private void Jump()
		{
			if (_canJump)
			{
				ChangeMoveState(EMove.Jump);
			}
		}

		// TODO Evasion animation
		// If the player does not move then play an evasion animation
		// When the player moves, play a dodge animation
		
		// Speed up the animation
		
		// Separate evasion animations when locked on enemy
		// * forward			| default forward roll
		// * left, right, back	| a small leap in the direction
		
		// FIXME Rotate player when spam dodge
		// Roll may have the wrong direction
		private void Dodge()
		{
			if (_canDodge && ChangeMoveState(EMove.Dodge))
			{
				_canDodge = false;
				StartCoroutine(DodgeDelay());
			}
		}

		private void Sprint(bool pressed)
		{	
			// if (_move == EMove.Block) _requiredSpeed = pressed ? _runSpeed : _walkSpeed;
			// if (_move == EMove.Move) _requiredSpeed = pressed ? _sprintSpeed : _runSpeed;
			_requiredSpeed = pressed ? _sprintSpeed : _runSpeed;
		}
		
		// TODO Realize block
		// TODO Special animation for movement when block
		private void Block(bool pressed)
		{
			if (pressed) ChangeMoveState(EMove.Block);
			else if (_move != EMove.Dodge) ChangeMoveState();
		}

		// TODO Start moving before the attack ends
		// take the animation time from combo system ??
		private void IsAttacking(bool isAttacking)
		{
			if (isAttacking) ChangeMoveState(EMove.Attack);
			else ChangeMoveState();
		}

		private void CheckLand()
		{
			if (_isLanded && !IsGrounded())
			{
				_isLanded = false;
				ChangeMoveState(EMove.Fall);
			}

			if (!_isLanded && IsGrounded())
			{
				_isLanded = true;
				_ySpeed = Physics.gravity.y;
				ChangeMoveState();
				if (_controls.Gameplay.Block.IsPressed()) Block(true); 
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
			ChangeMoveState();
			_canDodge = true;
		}

		private void InvokeSteps()
		{
			// if ((_forwardDirection.x != 0 || _forwardDirection.z != 0) && _isLanded && _canDodge && !_isAttackGoing)
			// {
			// 	PlaySteps?.Invoke(true);
			// }
			// else
			// {
			// 	PlaySteps?.Invoke(false);
			// }
		}

		public Vector3 GetForwardDirection()
		{
			var direction = _inputController.MoveDirection;
			_forwardDirection = direction.y * _camera.forward + direction.x * _camera.right;
			return _forwardDirection.normalized;
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
			if (_move == EMove.Dodge || _move == EMove.Jump || _move == EMove.Fall) return false;
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
