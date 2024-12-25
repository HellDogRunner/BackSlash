using Scripts.Player;
using UnityEngine;
using Zenject;
using Scripts.Weapon;
using Scripts.Player.camera;

namespace Scripts.Animations
{
	public class PlayerAnimationController : MonoBehaviour
	{
		[SerializeField] private Animator _animator;
		[SerializeField] private AnimatorOverrideController _swordOverride;
		[SerializeField] private AnimatorOverrideController _mainOverride;
		[SerializeField] private float _smoothBlend;
		private MovementController _movementController;
		private TargetLock _targetLock;
		private PlayerStateController _playerState;
		private WeaponController _weaponController;

		[Inject]
		private void Construct(PlayerStateController playerState, MovementController movementController, TargetLock targetLock, WeaponController weaponController, ThirdPersonCameraController thirdPersonController)
		{
			_movementController = movementController;
			_weaponController = weaponController;
			_targetLock = targetLock;
			_playerState = playerState;
		}

		private void Awake()
		{
			_weaponController.OnWeaponEquip += ShowWeapon;
			
			_targetLock.OnSwitchLock += SwitchLock;

			_movementController.OnJump += Jump;
			_movementController.InAir += InAir;
			_movementController.OnSprint += Sprint;
			_movementController.OnLockMove += LockMove;
			_movementController.OnFreeMove += FreeMove;
			_movementController.OnTryMove += TryMove;
			
			_playerState.OnAttack += PrimaryAttack;
			_playerState.OnBlock += Block;
			_playerState.OnDodge += Dodge;
		}

		private void OnDestroy()
		{
			_weaponController.OnWeaponEquip -= ShowWeapon;
			
			_targetLock.OnSwitchLock -= SwitchLock;

			_movementController.OnJump -= Jump;
			_movementController.InAir -= InAir;
			_movementController.OnSprint -= Sprint;
			_movementController.OnLockMove -= LockMove;
			_movementController.OnFreeMove -= FreeMove;
			_movementController.OnTryMove -= TryMove;
			
			_playerState.OnAttack -= PrimaryAttack;
			_playerState.OnBlock -= Block;
			_playerState.OnDodge -= Dodge;
		}

		private void LockMove(Vector2 direction)
		{
			_animator.SetFloat("InputX", direction.x, _smoothBlend, Time.deltaTime);
			_animator.SetFloat("InputY", direction.y, _smoothBlend, Time.deltaTime);
		}

		private void FreeMove(float speed)
		{
			_animator.SetFloat("Speed", speed, _smoothBlend, Time.deltaTime);
		}

		private void TryMove(bool move)
		{
			_animator.SetBool("Move", move);
		}

		private void SwitchLock(bool value)
		{
			_animator.SetBool("TargetLock", value);
		}

		private void Sprint(bool isPressed)
		{
			_animator.SetBool("IsSprint", isPressed);
		}

		private void Jump()
		{
			_animator.Play("Jump");
		}

		private void InAir(bool isInAir)
		{
			_animator.SetBool("InAir", isInAir);
			_animator.applyRootMotion = !isInAir;
		}

		private void Dodge()
		{
			
			_animator.SetTrigger("Dodge");
		}

		private void ShowWeapon(bool equip)
		{
			_animator.runtimeAnimatorController = equip ? _swordOverride : _mainOverride;
			_animator.SetBool("Armed", equip);
			_animator.SetTrigger("Equip");
		}

		private void PrimaryAttack(bool isAttacking)
		{
			_animator.SetBool("Attacking", isAttacking);
		}

		private void ComboAttackAnimation()
		{
			_animator.SetTrigger("Combo");
		}

		private void JumpComboAttackAnimation()
		{
			_animator.SetTrigger("JumpCombo");
		}

		private void Block(bool isBlocking)
		{
			_animator.SetBool("Block", isBlocking);
		}

		public void TriggerAnimationByName(string name)
		{
			_animator.SetTrigger(name);
		}
	}
}