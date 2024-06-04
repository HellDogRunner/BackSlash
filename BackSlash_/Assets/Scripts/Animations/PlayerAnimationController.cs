using Scripts.Player;
using System;
using UnityEngine;
using Zenject;
using UnityEngine.Animations.Rigging;
using Scripts.Weapon;

namespace Scripts.Animations
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _smoothBlend = 0.1f;
        [SerializeField] private AnimatorOverrideController _swordOverride;
        [SerializeField] private AnimatorOverrideController _mainOverride;

        private InputController _inputService;
        private WeaponController _weaponController;
        private MovementController _movementController;
        private TargetLock _targetLock;

        public bool IsAttacking;
        [Inject]
        private void Construct(InputController inputService, WeaponController weaponController, MovementController movementService, TargetLock targetLock)
        {
            _inputService = inputService;
            _weaponController = weaponController;
            _movementController = movementService;
            _targetLock = targetLock;

            _movementController.OnJump += JumpAnimation;
            _movementController.OnDogde += DodgeAnimation;
            _movementController.IsMoving += SetIsMovingState;

            _inputService.OnSprintKeyPressed += SprintAnimation;
            _inputService.OnSprintKeyRealesed += RunAnimation;
            _inputService.OnShowWeaponPressed += ShowWeaponAnimation;
            _inputService.OnHideWeaponPressed += HideWeaponAnimation;
            _inputService.OnBlockPressed += BlockAnimation;
            _inputService.OnWeaponIdle += WeaponIdle;
            _weaponController.OnAttack += AttackAnimation;
        }

        private void OnDestroy()
        {
            _movementController.OnJump -= JumpAnimation;
            _movementController.OnDogde -= DodgeAnimation;
            _movementController.IsMoving -= SetIsMovingState;

            _inputService.OnSprintKeyPressed -= SprintAnimation;
            _inputService.OnSprintKeyRealesed -= RunAnimation;
            _inputService.OnShowWeaponPressed -= ShowWeaponAnimation;
            _inputService.OnHideWeaponPressed -= HideWeaponAnimation;
            _inputService.OnBlockPressed -= BlockAnimation;
            _inputService.OnWeaponIdle -= WeaponIdle;
            _weaponController.OnAttack -= AttackAnimation;
        }

        private void Update()
        {
            var dir = _inputService.MoveDirection;
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

        private void JumpAnimation()
        {
            _animator.Play("Jump forward");
        }

        private void DodgeAnimation()
        {
            _animator.Play("Dodge");
        }

        private void ShowWeaponAnimation()
        {
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Show)
            {
                _animator.SetTrigger("Equip");
                _animator.runtimeAnimatorController = _swordOverride;
            }
        }

        private void HideWeaponAnimation()
        {
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Hide)
            {
                _animator.SetTrigger("Unequip");
                _animator.runtimeAnimatorController = _mainOverride;
            }
        }

        private void AttackAnimation(int currentAttack)
        {
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Attack)
            {
                if (_weaponController.CurrentWeaponType == EWeaponType.Melee)
                {
                    _animator.SetTrigger("Attack" + currentAttack);
                    IsAttacking = true;
                }
            }
        }

        private void SetIsMovingState(bool isMove)
        {
            _animator.SetBool("Moving", isMove);
        }

        private void BlockAnimation()
        {
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Block)
            {
                _animator.SetBool("Block", true);
            }
        }

        private void WeaponIdle()
        {
            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Idle)
            {
                _animator.SetBool("Block", false);
                IsAttacking = false;
            }
        }
        public bool IsAnimationPlaying(string animationName)
        {
            var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(1);
            if (animatorStateInfo.IsName(animationName))
                return true;

            return false;
        }
    }
}