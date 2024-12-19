using System;
using System.Collections;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	public class MovementController : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;
		[Header("Monitoring")]
		[SerializeField] private float _currentForce;
		[SerializeField] private Vector3 _moveDirection;
		[Header("Settings")]
		[SerializeField] private float _walkForce;
		[SerializeField] private float _runForce;
		[SerializeField] private float _sprintForce;
		[SerializeField] private float _moveMulti;
		[SerializeField] private float _standMulti;
		[Space]
		[SerializeField] private float _jumpForce;
		[SerializeField] private float _jumpDelay;
		[Space]
		[SerializeField] private float _dodgeCooldown;
		[SerializeField] private float _dodgeDelay;
		[Space]
		[SerializeField] private float _airForce;
		[SerializeField] private float _airMulti;
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
		private bool _canJump = true;
		private bool _canDodge = true;

		private float _requiredForce;
		private float _yForce;

		private Vector3 _forwardDirection;
		private Vector3 _lockedDirection;

		private InputController _inputController;
		private ComboSystem _comboSystem;
		private Transform _camera;

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
			_requiredForce = _runForce;
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

			if (direction == Vector2.zero)
			{
				_tryMove = false;
				_requiredForce = 0;
			}
			else
			{
				_tryMove = true;
				_requiredForce = _runForce;
			}	
		}

		private void MovePlayer()
		{	
			Vector3 direction;
			OnMoving?.Invoke(_inputController.MoveDirection);
			
			if (_inAir)
			{
				_yForce += Physics.gravity.y * Time.deltaTime * _gravityMulti;
				_lockedDirection = TryNormalize(_lockedDirection + GetMoveDirection() * _airDirectionMulti);
				direction = _lockedDirection * _currentForce;
			}
			else
			{
				if (!_tryMove && _currentForce != _requiredForce || _move == EMove.Attack)
				{
					_moveDirection /= _currentForce;
					ChangeForce(_standMulti);
					direction = _moveDirection * _currentForce;
				}
				else
				{
					ChangeForce(_moveMulti);
					direction = GetMoveDirection() * _currentForce;
				}
			}
			
			if (_move == EMove.Dodge) direction = _lockedDirection;
			
			direction.y = _yForce;
			_moveDirection = direction;
			_characterController.Move(_moveDirection * Time.deltaTime);
		}
		
		private void MoveState(EMove move = EMove.None)
		{
			switch (move)
			{
				case EMove.None:
					_requiredForce = _runForce;
					break;
				
				case EMove.Attack:
					_requiredForce = 0;
					break;
				
				case EMove.Dodge:
					_lockedDirection = GetMoveDirection() * _runForce;
					_lockedDirection.y = _yForce;
					break;
				
				case EMove.Block:
					_requiredForce = _walkForce;
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
		
		private void ChangeForce(float multi)
		{
			float coef;
			if (_currentForce == _requiredForce) return;
			else if (_currentForce < _requiredForce) coef = 1;
			else coef = -1;
			
			_currentForce += coef * Time.deltaTime * 10 * multi;
			
			if (Math.Abs(_requiredForce - _currentForce) < 0.5f) _currentForce = _requiredForce;
		}
		
		private void Jump()
		{
			if (!_inAir && _canJump && _move != EMove.Dodge && _move != EMove.Attack)
			{
				_canJump = false;
				_isJump = true;
				OnJump?.Invoke();
				_yForce = _jumpForce;
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
			_requiredForce = pressed ? _sprintForce : _runForce;
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

		private void CheckLand()
		{
			if (!IsGrounded() && !_inAir)
			{
				_currentForce = _airForce;
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
