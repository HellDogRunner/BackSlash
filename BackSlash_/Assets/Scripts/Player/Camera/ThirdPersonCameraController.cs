using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Player.camera
{
	class ThirdPersonCameraController : MonoBehaviour
	{
		[SerializeField] private Transform _lookAt;
		
		[Header("References")]
		[SerializeField] private float _rotationTime;
		[SerializeField] private float _turnTime;
		[SerializeField] private float _delayRotationTime;

		private float _timeToRotate;

		private MovementController _movement;
		private InputController _inputController;
		private TargetLock _targetLock;
		private ComboSystem _comboSystem;

		private Transform _camera;
		private Coroutine _currentDelayRoutine;

		private bool _isAttacking;

		public event Action<bool> IsAttacking;

		[Inject]
		private void Construct(MovementController movement, InputController inputController, TargetLock targetLock, ComboSystem comboSystem)
		{
			_movement = movement;
			_inputController = inputController;
			_targetLock = targetLock;
			_comboSystem = comboSystem;
		}

		private void OnEnable()
		{
			_comboSystem.IsAttacking += OnAttack;
		}

		private void OnDisable()
		{
			_comboSystem.IsAttacking -= OnAttack;
		}

		private void Awake()
		{
			_camera = Camera.main.transform;
		}

		private void FixedUpdate()
		{
			_timeToRotate += Time.deltaTime;
			
			if (_timeToRotate >= _delayRotationTime) 
			{
				_timeToRotate = 0;
				_isAttacking = false;
			}

			IsAttacking?.Invoke(_isAttacking);
		}
		
		// TODO delay before RotateToTarget() [?!!]
		
		private void Update()
		{
			if (_targetLock.Target != null)
			{
				if (_movement.CanRotate() && _movement.IsSprint) RotatePlayer();
				else RotateToTarget();
				_lookAt.LookAt(_targetLock.Target.transform.position);
			}
			else
			{
				if (_isAttacking) RotatePlayerForward();
				else if (_movement.CanRotate()) RotatePlayer();
			}
		}

		private void RotatePlayer()
		{
			var direction = _inputController.MoveDirection;

			if (direction != Vector2.zero)
			{
				Vector3 moveDirection = direction.y * _camera.forward + direction.x * _camera.right;
				moveDirection.y = 0;
				transform.forward = Vector3.Lerp(moveDirection, transform.forward, _rotationTime);
			}
		}

		private void RotateToTarget()
		{
			var target = _targetLock.Target.transform.position - gameObject.transform.position;
			target.y = 0;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target), _turnTime);
		}

		private void RotatePlayerForward()
		{
			float cameraYaw = _camera.eulerAngles.y;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, cameraYaw, 0), _turnTime);
		}

		private void OnAttack(bool attack) 
		{
			if (attack)
			{
				_isAttacking = true;
				_timeToRotate = 0;
			}
		}

		private IEnumerator DelayRotation(float delaySecounds)
		{
			yield return new WaitForSeconds(delaySecounds);

			_isAttacking = false;
		}
	}
}
