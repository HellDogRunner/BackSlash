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
		
		private bool _fall;
		
		private MovementController _movementController;
		private WeaponController _weaponController;
		private InputController _inputController;
		private TargetLock _targetLock;

		[Inject]
		private void Construct(InputController inputController, MovementController movementController, TargetLock targetLock, WeaponController weaponController, CameraController thirdPersonController)
		{
			_movementController = movementController;
			_weaponController = weaponController;
			_inputController = inputController;
			_targetLock = targetLock;
		}

		private void Awake()
		{
			_movementController.OnLockMove += LockMove;
			_movementController.OnFreeMove += FreeMove;
			_movementController.OnTryMove += TryMove;
			_movementController.OnLanding += Landing;
			_movementController.OnSprint += Sprint;
			_movementController.OnJump += Jump;
			_movementController.InAir += InAir;
			_movementController.OnFall += Fall;
			
			_weaponController.OnWeaponEquip += ShowWeapon;
			
			_targetLock.OnSwitchLock += SwitchLock;
		}

		private void OnDestroy()
		{
			_movementController.OnLockMove -= LockMove;
			_movementController.OnFreeMove -= FreeMove;
			_movementController.OnTryMove -= TryMove;
			_movementController.OnLanding -= Landing;
			_movementController.OnSprint -= Sprint;
			_movementController.OnJump -= Jump;
			_movementController.InAir -= InAir;
			_movementController.OnFall -= Fall;
			
			_weaponController.OnWeaponEquip -= ShowWeapon;
			
			_targetLock.OnSwitchLock -= SwitchLock;
		}

		private void Update()
		{
			if (_fall)
			{
				_animator.SetFloat("Fall Speed", 1, _smoothFall, Time.deltaTime);
				if (!_animator.GetBool("InAir")) _fall = false;
			}
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
			_animator.SetInteger("Input Direction", CalculateDirection());
			_animator.SetTrigger("Jump");
			_animator.applyRootMotion = false;
		}

		public void Fall()
		{
			_fall = true;
			_animator.SetFloat("Fall Speed", 0);
			_animator.SetTrigger("Fall");
		}
		
		private void Landing()
		{
			_animator.SetTrigger("Landing");
		}
		
		private void InAir(bool inAir)
		{
			_animator.SetBool("InAir", inAir);
			if (!inAir) _animator.applyRootMotion = true;
		}
		
		public void Dodge()
		{
			_animator.SetInteger("Input Direction", CalculateDirection());
			_animator.SetTrigger("Dodge");
		}

		private void ShowWeapon(bool equip)
		{
			_animator.runtimeAnimatorController = equip ? _swordOverride : _mainOverride;
			_animator.SetBool("Armed", equip);
			_animator.SetTrigger("Equip");
		}

		public void Attack(bool isAttacking)
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

		public void Block(bool isBlocking)
		{
			_animator.SetBool("Block", isBlocking);
		}

		public void TriggerAnimationByName(string name)
		{
			_animator.SetTrigger(name);
		}
		
		private int CalculateDirection()
		{
			var direction = _inputController.MoveDirection;
			int value;
			
			if (direction == Vector2.zero) return 0;
			
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
	}
}
