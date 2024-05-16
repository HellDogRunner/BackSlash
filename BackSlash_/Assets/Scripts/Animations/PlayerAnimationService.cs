using Scripts.Player;
using System;
using UnityEngine;
using Zenject;
using UnityEngine.Animations.Rigging;
using Scripts.Weapon;

namespace Scripts.Animations
{
    public class PlayerAnimationService : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _smoothBlend = 0.1f;
        [SerializeField] private AnimatorOverrideController _swordOverride;
        [SerializeField] private AnimatorOverrideController _mainOverride;

        float ikWeight = 1f;
        [SerializeField] float footPlacementOffset;

        private InputService _inputService;
        private WeaponController _weaponController;

        [Inject]
        private void Construct(InputService inputService, WeaponController weaponController)
        {
            _inputService = inputService;
            _weaponController = weaponController;

            _inputService.OnSprintKeyPressed += SprintAnimation;
            _inputService.OnSprintKeyRealesed += RunAnimation;
            _inputService.OnJumpKeyPressed += JumpAnimation;
            _inputService.OnDogdeKeyPressed += DodgeAnimation;
            _inputService.OnShowWeaponPressed += ShowWeaponAnimation;
            _inputService.OnHideWeaponPressed += HideWeaponAnimation;
            _inputService.OnBlockPressed += BlockAnimation;
            _inputService.OnWeaponIdle += WeaponIdle;
            _weaponController.OnAttack += AttackAnimation;
        }

        private void OnDestroy()
        {
            _inputService.OnSprintKeyPressed -= SprintAnimation;
            _inputService.OnSprintKeyRealesed -= RunAnimation;
            _inputService.OnJumpKeyPressed -= JumpAnimation;
            _inputService.OnDogdeKeyPressed -= DodgeAnimation;
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
                }
            }
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
            }
        }
    }
}