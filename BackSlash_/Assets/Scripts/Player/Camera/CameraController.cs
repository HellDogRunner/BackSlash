using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Scripts.Player.camera
{
public class CameraController : MonoBehaviour
	{
		[SerializeField] private CinemachineCamera _lockCamera;
		[SerializeField] private CinemachineRotationComposer _rotationComposer;
		[SerializeField] private Transform _lookAt;
		
		[Header("Rotation")]
		[SerializeField] private float _freeTurnTime;
		[SerializeField] private float _lockedTurnTime;
		[SerializeField] private float _slowedTurnTime;

		[Header("Camera")]
		[SerializeField] private float _targetingDelay;
		[SerializeField] private float _screenPositionY;
		[SerializeField] private float _distance;
		
		private PlayerStateController _stateController;
		private InputController _inputController;
		private MovementController _movement;
		private TargetLock _targetLock;

		private Transform _camera;
		private Transform _target;
		
		private bool _isTargeting;
		private bool _isAttacking;
		private bool _canRotate;

		[Inject]
		private void Construct(MovementController movement, PlayerStateController stateController, InputController inputController, TargetLock targetLock)
		{
			_inputController = inputController;
			_stateController = stateController;
			_movement = movement;
			_targetLock = targetLock;
		}

		private void OnEnable()
		{
			_targetLock.OnSwitchLock += SwitchLockCamera;
		}

		private void OnDisable()
		{
			_targetLock.OnSwitchLock -= SwitchLockCamera;
		}

		private void Awake()
		{
			_canRotate = true;
			_camera = Camera.main.transform;
		}
		
		private void Update()
		{
			if (_isTargeting)
			{
				var distance = (transform.position - _target.position).magnitude;
				if (distance < _distance)
				{
					_rotationComposer.Composition.ScreenPosition.y = _screenPositionY * distance / _distance;
				}
				else _rotationComposer.Composition.ScreenPosition.y = _screenPositionY;
				
				if (_movement.CanLockedRotate()) RotatePlayer();
				else RotateToTarget();
			}
			else
			{
				if (_isAttacking) RotatePlayerForward();
				else RotatePlayer();	
			}
			
			if (_target) _lookAt.LookAt(_target.position);
		}

		private void RotatePlayer()
		{
			var direction = _inputController.MoveDirection;

			if (direction != Vector2.zero && _canRotate)
			{
				var time = _stateController.SlowedRotate() || _movement.Air ? _slowedTurnTime : _freeTurnTime;
				Vector3 moveDirection = direction.y * _camera.forward + direction.x * _camera.right;
				moveDirection.y = 0;
				transform.forward = Vector3.Lerp(transform.forward, moveDirection, time * Time.deltaTime);
			}
		}

		private void RotateToTarget()
		{
			var target = _target.position - gameObject.transform.position;
			target.y = 0;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target), _lockedTurnTime * Time.deltaTime);
		}

		private void RotatePlayerForward()
		{
			float cameraYaw = _camera.eulerAngles.y;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, cameraYaw, 0), _lockedTurnTime * Time.deltaTime);
		}

		private void SwitchLockCamera(bool value)
		{
			if (value)
			{
				_target = _targetLock.Target.transform;
				_lockCamera.Target.LookAtTarget = _targetLock.Target.LookAt;
			}
			else StartCoroutine(DelayTargeting());
			
			_lockCamera.gameObject.SetActive(value);
			_isTargeting = value;
		}

		private IEnumerator DelayTargeting()
		{
			yield return new WaitForSeconds(_targetingDelay);
			_target = null;
		}
		
		public void SetCanRotate(bool value)
		{
			_canRotate = value;
		}
		
		public void SetAttack(bool value)
		{
			_isAttacking = value;
		}
	}
}
