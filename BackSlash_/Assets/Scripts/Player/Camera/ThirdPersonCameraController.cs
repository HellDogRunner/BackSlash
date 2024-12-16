using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Player.camera
{
	class ThirdPersonCameraController : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Transform _baseOrientation;
		[SerializeField] private float _rotationTime = 30;
		[SerializeField] private float _turnSpeed = 30;
		[SerializeField] private float _delayRotationTime = 5f;

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
			_comboSystem.IsAttacking += OnAttack;

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

			if (_targetLock.Target != null) RotatePlayerLocked();
			else
			{
				if (_movement.CanRotate())
				{	
					if (_isAttacking) RotatePlayerForward();
					else RotatePlayer();
				}
			}
			
			IsAttacking?.Invoke(_isAttacking);
		}

		private void RotatePlayer()
		{
			var direction = _inputController.MoveDirection;
			Vector3 ViewDir = gameObject.transform.position - new Vector3(_camera.position.x, gameObject.transform.position.y, _camera.position.z);
			_baseOrientation.forward = ViewDir.normalized;

			Vector3 inputDir = _baseOrientation.forward * direction.y + _baseOrientation.right * direction.x;
			if (inputDir != Vector3.zero)
			{
				gameObject.transform.forward = Vector3.Slerp(inputDir, gameObject.transform.forward, _rotationTime * Time.deltaTime);
			}
		}

		private void RotatePlayerLocked()
		{
			Vector3 rotationDirection = _targetLock.Target.transform.position - gameObject.transform.position;
			rotationDirection.Normalize();
			rotationDirection.y = 0;
			Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationTime * Time.deltaTime);
		}

		private void RotatePlayerForward()
		{
			float cameraYaw = _camera.eulerAngles.y;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, cameraYaw, 0), _turnSpeed * Time.deltaTime);
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

		private void OnDestroy()
		{
			_comboSystem.IsAttacking -= OnAttack;
		}
	}
}