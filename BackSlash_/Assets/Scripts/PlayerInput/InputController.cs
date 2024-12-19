using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
	public class InputController : MonoBehaviour
	{
		private GameControls _playerControls;

		private Vector3 _moveDirection;

		public event Action<bool> OnSprintKeyPressed;
		public event Action<bool> OnLightAttackPressed;
		public event Action<bool> OnHeavyAtttackPressed;
		public event Action<bool> OnBlockPressed;
		public event Action OnShowWeaponPressed;
		public event Action OnJumpKeyPressed;
		public event Action OnDodgeKeyPressed;
		public event Action OnLockKeyPressed;

		public Vector3 MoveDirection => _moveDirection;

		private void Awake()
		{
			_playerControls = new GameControls();
		}

		private void OnEnable()
		{
			_playerControls.Enable();
			SubscribeToActions();
		}

		private void OnDisable()
		{
			_playerControls.Disable();
			UnsubscribeToActions();
			_moveDirection = Vector3.zero;
		}

		private void ChangeDirection(InputAction.CallbackContext context)
		{
			var direction = _playerControls.Gameplay.WASD.ReadValue<Vector3>();
			_moveDirection = new Vector3(direction.x, direction.z, direction.y);
		}

		private void ShowWeapon(InputAction.CallbackContext context)
		{
			OnShowWeaponPressed?.Invoke();
		}

		private void Sprint(InputAction.CallbackContext context)
		{
			var isPressed = _playerControls.Gameplay.Sprint.IsPressed();
			OnSprintKeyPressed?.Invoke(isPressed);
		}

		private void Dodge(InputAction.CallbackContext context)
		{
			OnDodgeKeyPressed?.Invoke();
		}

		private void LightAttack(InputAction.CallbackContext context)
		{
			var isPressed = _playerControls.Gameplay.LightAttack.IsPressed();
			OnLightAttackPressed?.Invoke(isPressed);
		}

		private void HeavyAttackPressed(InputAction.CallbackContext context)
		{
			var isPressed = _playerControls.Gameplay.HeavyAttack.IsPressed();
			OnHeavyAtttackPressed?.Invoke(isPressed);
		}

		private void Jump(InputAction.CallbackContext context)
		{
			OnJumpKeyPressed?.Invoke();
		}

		private void Block(InputAction.CallbackContext context)
		{
			var isPressed = _playerControls.Gameplay.Block.IsPressed();
			OnBlockPressed?.Invoke(isPressed);
		}

		private void Lock(InputAction.CallbackContext context)
		{
			OnLockKeyPressed?.Invoke();
		}

		private void SubscribeToActions()
		{
			_playerControls.Gameplay.WASD.performed += ChangeDirection;
			_playerControls.Gameplay.Dodge.performed += Dodge;
			_playerControls.Gameplay.LightAttack.performed += LightAttack;
			_playerControls.Gameplay.HeavyAttack.started += HeavyAttackPressed;
			_playerControls.Gameplay.Sprint.performed += Sprint;
			_playerControls.Gameplay.Block.performed += Block;
			_playerControls.Gameplay.Jump.performed += Jump;
			_playerControls.Gameplay.ShowWeapon.performed += ShowWeapon;
			_playerControls.Gameplay.TargetLock.performed += Lock;
		}

		private void UnsubscribeToActions()
		{
			_playerControls.Gameplay.WASD.performed -= ChangeDirection;
			_playerControls.Gameplay.Dodge.performed -= Dodge;
			_playerControls.Gameplay.LightAttack.performed -= LightAttack;
			_playerControls.Gameplay.HeavyAttack.started -= HeavyAttackPressed;
			_playerControls.Gameplay.Sprint.performed -= Sprint;
			_playerControls.Gameplay.Block.performed -= Block;
			_playerControls.Gameplay.Jump.performed -= Jump;
			_playerControls.Gameplay.ShowWeapon.performed -= ShowWeapon;
			_playerControls.Gameplay.TargetLock.performed -= Lock;
		}
	}
}
