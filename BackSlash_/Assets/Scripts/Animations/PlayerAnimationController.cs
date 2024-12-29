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
		[Space]
		[SerializeField] private float _smoothBlend;
		[SerializeField] private float _smoothFreeMove;
		[SerializeField] private float _smoothFall;
		
		private Vector2 _dodgeDirection;
		
		private MovementController _movementController;
		private PlayerStateController _playerState;
		private WeaponController _weaponController;
		private InputController _inputController;
		private TargetLock _targetLock;

		[Inject]
		private void Construct(InputController inputController, PlayerStateController playerState, MovementController movementController, TargetLock targetLock, WeaponController weaponController, CameraController thirdPersonController)
		{
			_movementController = movementController;
			_weaponController = weaponController;
			_inputController = inputController;
			_playerState = playerState;
			_targetLock = targetLock;
		}

		private void Awake()
		{
			_movementController.OnLockMove += LockMove;
			_movementController.OnFreeMove += FreeMove;
			_movementController.OnTryMove += TryMove;
			_movementController.OnFalling += Falling;
			_movementController.OnLanding += Landing;
			_movementController.OnSprint += Sprint;
			_movementController.OnJump += Jump;
			_movementController.InAir += InAir;
			_movementController.OnFall += Fall;
			
			_playerState.OnAttack += PrimaryAttack;
			_playerState.OnBlock += Block;
			_playerState.OnDodge += Dodge;
			
			_weaponController.OnWeaponEquip += ShowWeapon;
			
			_targetLock.OnSwitchLock += SwitchLock;
		}

		private void OnDestroy()
		{
			_movementController.OnLockMove -= LockMove;
			_movementController.OnFreeMove -= FreeMove;
			_movementController.OnTryMove -= TryMove;
			_movementController.OnFalling -= Falling;
			_movementController.OnLanding -= Landing;
			_movementController.OnSprint -= Sprint;
			_movementController.OnJump -= Jump;
			_movementController.InAir -= InAir;
			_movementController.OnFall -= Fall;
			
			_playerState.OnAttack -= PrimaryAttack;
			_playerState.OnBlock -= Block;
			_playerState.OnDodge -= Dodge;
			
			_weaponController.OnWeaponEquip -= ShowWeapon;
			
			_targetLock.OnSwitchLock -= SwitchLock;
		}

		private void LockMove(Vector2 direction)
		{
			_animator.SetFloat("InputX", direction.x, _smoothBlend, Time.deltaTime);
			_animator.SetFloat("InputY", direction.y, _smoothBlend, Time.deltaTime);
		}

		private void FreeMove(float speed)
		{
			_animator.SetFloat("Speed", speed, _smoothFreeMove, Time.deltaTime);
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
			_animator.SetTrigger("Jump");
		}
		
		private void Falling()
		{
			_animator.SetFloat("Fall Speed", 1, _smoothFall, Time.deltaTime);
		}
		
		private void Landing()
		{
			_animator.SetTrigger("Landing");
		}
		
		private void Fall()
		{
			_animator.SetFloat("Fall Speed", 0);
			_animator.SetTrigger("Fall");
		}
		
		private void InAir(bool isInAir)
		{
			_animator.SetBool("InAir", isInAir);
			_animator.applyRootMotion = !isInAir;
		}
		
		private int CalculateDirection()
		{
			var direction = _inputController.MoveDirection;
			int value;
			
			if (direction == Vector2.zero) return 3;
			
			if (direction.x != 0)
			{
				value = direction.x == -1 ? 1 : 2;
			}
			else
			{
				value = direction.y == 1 ? 3 : 4;
			}
			
			return value;
		}
		
		private void Dodge()
		{
			_animator.SetInteger("DodgeD", CalculateDirection());
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
