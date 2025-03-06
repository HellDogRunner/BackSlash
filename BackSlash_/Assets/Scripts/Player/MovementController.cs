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
		[SerializeField] private float _jumpTime;
		[SerializeField] private float _jumpHeight;
		[SerializeField] private float _jumpDelay;
		[Space]
		[SerializeField] private float _airRunSpeed;
		[SerializeField] private float _airSpintSpeed;
		[SerializeField] private float _airDirectionMulti;
		[Space]
		[SerializeField] private float _fallDelay;

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
		private bool _canMove;
		private bool _canSprint;

		private float _gravityForce;
		private float _startJumpVelocity;
		private float _requiredSpeed;
		private float _ySpeed;
		private float _airSpeed;

		private Vector3 _airDirection;
		private Vector2 _inputDirection;

		private Transform _camera;
		private TargetLock _targetLock;
		private InputController _inputController;
		
		private Coroutine _fallRoutine;

		public bool CanMove => _canMove;
		public bool Jumping => _inJump;
		public bool Air => _inAir;
		public Vector2 InputDirection => _inputDirection;

		public event Action<Vector2> OnLockMove;
		public event Action<float> OnFreeMove;
		public event Action<bool> OnSetMove;
		public event Action<bool> PlaySteps;
		public event Action<bool> OnSprint;
		public event Action<bool> InAir;
		public event Action OnFall;
		public event Action OnJump;

		[Inject]
		private void Construct(TargetLock targetLock, InputController inputController)
		{
			_inputController = inputController;
			_targetLock = targetLock;
		}

		private void Awake()
		{
			_canMove = true;
			_camera = Camera.main.transform;
		}

		private void Start()
		{
			CalculateJump();
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
			
			if (_canFall && _inAir && _fallRoutine == null)
			{
				_fallRoutine = StartCoroutine(FallDelay(_fallDelay));
			}
		}

		private void Move()
		{
			_inputDirection = _inputController.MoveDirection;
			_tryMove = _inputDirection != Vector2.zero;
			if (_canMove) OnSetMove?.Invoke(_tryMove);
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
				if (_ySpeed <= Physics.gravity.y)
				{
					_ySpeed = Physics.gravity.y;
				} 
				else
				{
					_ySpeed -= _gravityForce * Time.deltaTime;
				}
				
				direction = _airDirection * _airSpeed + GetInputDirection() * _airDirectionMulti;
			}
			else
			{
				if (!_inJump) _ySpeed = Physics.gravity.y;
			}

			direction.y = _ySpeed;
			_characterController.Move(direction * Time.deltaTime);
		}

		private void Jump()
		{
			if (!_inAir && !_inJump && _canJump && Time.timeScale != 0)
			{
				CalculateJump();
				CalculateAirSpeed();
				OnJump?.Invoke();
			}
		}
		
		private void JumpStart()
		{
			_inJump = true;
			_ySpeed = _startJumpVelocity;
			_fallRoutine = StartCoroutine(FallDelay(_jumpTime));
		}

		private void Sprint(bool pressed)
		{
			if (!pressed)
			{
			    _isSprint = false;
			}
			else if (_canSprint && !_inAir)
			{
				_isSprint = true;
			}
			
			OnSprint?.Invoke(_isSprint);
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
				_isSprint = false;
			}
		}
		
		public void SetCanMove(bool value)
		{
			_canMove = value;
		}
		
		private void CheckLand()
		{
			if (!IsGrounded() && !_inAir)
			{
				_airDirection = _targetLock.Target ? GetInputDirection() : GetJumpDirection();
				_inAir = true;
				InAir?.Invoke(true);
				
				if (!_inJump)
				{
					_ySpeed = 0;
					CalculateAirSpeed();
				}
			}

			if (IsGrounded() && _inAir)
			{
				_inAir = false;
				InAir?.Invoke(false);

				StopCoroutine(_fallRoutine);
				_fallRoutine = null;
				StartCoroutine(JumpDelay());
			}
		}

		private IEnumerator FallDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			OnFall?.Invoke();
		}

		private IEnumerator JumpDelay()
		{
			_inJump = true;
			yield return new WaitForSeconds(_jumpDelay);
			_inJump = false;
		}

		private void InvokeSteps()
		{
			PlaySteps?.Invoke(!_inAir && _tryMove);
		}

		private void CalculateAirSpeed()
		{
			if (_isSprint)
			{
			    _airSpeed = _airSpintSpeed;
			}
			else if (_tryMove && _canMove)
			{
			    _airSpeed = _airRunSpeed;
			}
			else
			{
			    _airSpeed = 0;
			}
		}

		private Vector3 GetInputDirection()
		{
			var direction = _inputController.MoveDirection;
			return (direction.y * _camera.forward + direction.x * _camera.right).normalized;
		}

		private Vector3 GetJumpDirection()
		{
			return _inputController.MoveDirection == Vector2.zero ? Vector2.zero : transform.forward.normalized;
		}

		private void CalculateJump()
		{
			float heightTime = _jumpTime / 2;
			_gravityForce = 2 * _jumpHeight / Mathf.Pow(heightTime, 2);
			_startJumpVelocity = 2 * _jumpHeight / heightTime;
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
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
