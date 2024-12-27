using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Player.camera
{
	class ThirdPersonCameraController : MonoBehaviour
	{
		[SerializeField] private Transform _lookAt;
		
		[Header("References")]
		[SerializeField] private float _freeTurnTime;
		[SerializeField] private float _lockedTurnTime;
		[SerializeField] private float _slowedTurnTime;
		//[SerializeField] private float _delayRotationTime;

		private float _timeToRotate;
		
		private PlayerStateController _stateController;
		private MovementController _movement;
		private InputController _inputController;
		private TargetLock _targetLock;
		private ComboSystem _comboSystem;

		private Transform _camera;
		private Coroutine _currentDelayRoutine;

		private bool _isAttacking;

		[Inject]
		private void Construct(PlayerStateController stateController, MovementController movement, InputController inputController, TargetLock targetLock, ComboSystem comboSystem)
		{
			_inputController = inputController;
			_stateController = stateController;
			_comboSystem = comboSystem;
			_targetLock = targetLock;
			_movement = movement;
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
			// TODO Rotate player after attack ?
			
			// _timeToRotate += Time.deltaTime;
			
			// if (_timeToRotate >= _delayRotationTime) 
			// {
			// 	_timeToRotate = 0;
			// 	_isAttacking = false;
			// }
		}
		
		// TODO delay before RotateToTarget() [?!!]
		private void Update()
		{
			if (_targetLock.Target != null)
			{
				if (_stateController.LockedRotate()) RotatePlayer(_freeTurnTime);
				else RotateToTarget();
				_lookAt.LookAt(_targetLock.Target.transform.position);
			}
			else
			{
				if (_isAttacking) RotatePlayerForward();
				else
				{
					if (_stateController.SlowedRotate()) RotatePlayer(_slowedTurnTime);
					else RotatePlayer(_freeTurnTime);	
				}
			}
		}

		private void RotatePlayer(float time)
		{
			var direction = _inputController.MoveDirection;

			if (direction != Vector2.zero)
			{
				Vector3 moveDirection = direction.y * _camera.forward + direction.x * _camera.right;
				moveDirection.y = 0;
				transform.forward = Vector3.Lerp(moveDirection, transform.forward, time);
			}
		}

		private void RotateToTarget()
		{
			var target = _targetLock.Target.transform.position - gameObject.transform.position;
			target.y = 0;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target), _lockedTurnTime);
		}

		private void RotatePlayerForward()
		{
			float cameraYaw = _camera.eulerAngles.y;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, cameraYaw, 0), _lockedTurnTime);
		}

		private void OnAttack(bool attack) 
		{
			_isAttacking = attack;
			
			// if (attack)
			// {
			// 	_isAttacking = true;
			// 	_timeToRotate = 0;
			// }
		}

		private IEnumerator DelayRotation(float delaySecounds)
		{
			yield return new WaitForSeconds(delaySecounds);
			_isAttacking = false;
		}
	}
}
