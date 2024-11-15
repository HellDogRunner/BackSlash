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
		[SerializeField] private float _smoothBlend = 0.1f;
		[SerializeField] private AnimatorOverrideController _swordOverride;
		[SerializeField] private AnimatorOverrideController _mainOverride;

		private InputController _inputController;
		private MovementController _movementController;
		private TargetLock _targetLock;
		private WeaponController _weaponController;
		private ThirdPersonCameraController _thirdPersonController;

		[Inject]
		private void Construct(InputController inputController, MovementController movementController, TargetLock targetLock, WeaponController weaponController, ThirdPersonCameraController thirdPersonController)
		{
			_weaponController = weaponController;
			_targetLock = targetLock;
			_movementController = movementController;
			_inputController = inputController;
			_thirdPersonController = thirdPersonController;
		}

		private void Awake()
		{
			_weaponController.OnWeaponEquip += ShowWeapon;
			_weaponController.OnBlocking += BlockAnimation;
			
			_targetLock.OnSwitchLock += SwitchStrafeAnimation;
			
			_movementController.OnJump += JumpAnimation;
			_movementController.InAir += InAirAnimation;
			_movementController.OnDodge += DodgeAnimation;
			_movementController.OnSprint += SprintAnimation;
			
			_thirdPersonController.IsAttacking += PrimaryAttackAnimation;
		}

		private void OnDestroy()
		{
			_weaponController.OnWeaponEquip -= ShowWeapon;
			_weaponController.OnBlocking -= BlockAnimation;
			
			_targetLock.OnSwitchLock -= SwitchStrafeAnimation;

			_movementController.OnJump -= JumpAnimation;
			_movementController.InAir -= InAirAnimation;
			_movementController.OnDodge -= DodgeAnimation;
			_movementController.OnSprint -= SprintAnimation;

			_thirdPersonController.IsAttacking -= PrimaryAttackAnimation;
		}

		private void Update()
		{
			var direction = _inputController.MoveDirection;
			_animator.SetFloat("InputX", direction.x, _smoothBlend, Time.deltaTime);
			_animator.SetFloat("InputY", direction.z, _smoothBlend, Time.deltaTime);
		}

		private void SwitchStrafeAnimation(bool value)
		{
			_animator.SetBool("TargetLock", value);
		}

		private void SprintAnimation(bool isPressed)
		{
			_animator.SetBool("IsSprint", isPressed);
		}

		private void JumpAnimation()
		{
			_animator.Play("Jump");
		}

		private void InAirAnimation(bool isInAir)
		{
			_animator.SetBool("InAir", isInAir);
			_animator.applyRootMotion = !isInAir;
		}

		private void DodgeAnimation()
		{
			_animator.Play("Dodge");
		}

		private void ShowWeapon(bool equip)
		{
			if (equip)
			{
				_animator.SetTrigger("Equip");
				_animator.runtimeAnimatorController = _swordOverride;
			}
			else 
			{
				_animator.SetTrigger("Unequip");
				_animator.runtimeAnimatorController = _mainOverride;
			}
		}

		private void PrimaryAttackAnimation(bool isAttacking)
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

		private void BlockAnimation(bool isBlocking)
		{
			_animator.SetBool("Block", isBlocking);
		}

		public void TriggerAnimationByName(string name)
		{
			_animator.SetTrigger(name);
		}
	}
}