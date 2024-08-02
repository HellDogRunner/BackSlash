using Scripts.Player;
using UnityEngine;
using Zenject;
using Scripts.Weapon;

namespace Scripts.Animations
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _smoothBlend = 0.1f;
        [SerializeField] private AnimatorOverrideController _swordOverride;
        [SerializeField] private AnimatorOverrideController _mainOverride;

        private InputController _inputController;
        private WeaponController _weaponController;
        private MovementController _movementController;
        private TargetLock _targetLock;

        [Inject]
        private void Construct(InputController inputController, WeaponController weaponController, MovementController movementController, TargetLock targetLock)
        {
            _targetLock = targetLock;

            _movementController = movementController;
            _movementController.OnJump += JumpAnimation;
            _movementController.InAir += JumpAnimation;
            _movementController.OnDodge += DodgeAnimation;

            _inputController = inputController;
            _inputController.OnSprintKeyPressed += SprintAnimation;
            _inputController.OnSprintKeyRealesed += RunAnimation;
            _inputController.OnShowWeaponPressed += ShowWeaponAnimation;
            _inputController.OnHideWeaponPressed += HideWeaponAnimation;
            _inputController.OnAttackFinished += WeaponIdle;

            _weaponController = weaponController;
            _weaponController.OnAttack += AttackAnimation;
            _weaponController.IsAttacking += RunAndAttackAnimation;
            _weaponController.IsBlocking += BlockAnimation;
        }

        private void OnDestroy()
        {
            _movementController.OnJump -= JumpAnimation;
            _movementController.InAir -= JumpAnimation;
            _movementController.OnDodge -= DodgeAnimation;

            _inputController.OnSprintKeyPressed -= SprintAnimation;
            _inputController.OnSprintKeyRealesed -= RunAnimation;
            _inputController.OnShowWeaponPressed -= ShowWeaponAnimation;
            _inputController.OnHideWeaponPressed -= HideWeaponAnimation;
            _inputController.OnAttackFinished -= WeaponIdle;

            _weaponController.OnAttack -= AttackAnimation;
            _weaponController.IsAttacking -= RunAndAttackAnimation;
            _weaponController.IsBlocking -= BlockAnimation;
        }

        private void Update()
        {
            var dir = _inputController.MoveDirection;
            _animator.SetFloat("InputX", dir.x, _smoothBlend, Time.deltaTime);
            _animator.SetFloat("InputY", dir.z, _smoothBlend, Time.deltaTime);
            if (_targetLock.CurrentTargetTransform != null)
            {
                _animator.SetBool("TargetLock", true);
            }
            else
            {
                _animator.SetBool("TargetLock", false);
            }
        }

        private void SprintAnimation()
        {
            _animator.SetBool("IsSprint", true);
        }

        private void RunAnimation()
        {
            _animator.SetBool("IsSprint", false);
        }

        private void RunAndAttackAnimation(bool isAttacking) 
        {
            _animator.SetBool("Attacking", isAttacking);
        }

        private void JumpAnimation()
        {
            _animator.Play("Jump");
        }

        private void JumpAnimation(bool isInAir)
        {
            _animator.SetBool("InAir", isInAir);
        }

        private void DodgeAnimation()
        {
            _animator.Play("Dodge");
        }

        private void ShowWeaponAnimation()
        {
            if (_weaponController.CurrentWeaponType == EWeaponType.None)
            {
                _animator.SetTrigger("Equip");
                _animator.runtimeAnimatorController = _swordOverride;
            }
        }

        private void HideWeaponAnimation()
        {
            if (_weaponController.CurrentWeaponType != EWeaponType.None)
            {
                _animator.SetTrigger("Unequip");
                _animator.runtimeAnimatorController = _mainOverride;
            }
        }

        private void AttackAnimation(int currentAttack)
        {
            if (_weaponController.CurrentWeaponType == EWeaponType.Melee)
            {
                _animator.SetTrigger("Attack" + currentAttack);
            }
        }

        private void BlockAnimation(bool isBlocking)
        {
            _animator.SetBool("Block", isBlocking);
        }

        private void WeaponIdle()
        {
            _animator.SetBool("Block", false);
        }
    }
}