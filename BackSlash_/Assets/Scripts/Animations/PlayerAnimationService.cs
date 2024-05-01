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
        [SerializeField] private float _smoothBlend;

        [SerializeField] private float _aimDuration;

        private InputService _inputService;
        private WeaponController _weaponController;


        [Inject]
        private void Construct(InputService inputService, WeaponController weaponController)
        {
            _inputService = inputService;
            _weaponController = weaponController;

            _weaponController.OnAttack += AttackAnimation;

            _inputService.OnSprintKeyPressed += SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed += JumpAnimation;
            _inputService.OnDogdeKeyPressed += DodgeAnimation;
            //_inputService.OnAttackPressed += AttackAnimation;
        }

        private void OnDestroy()
        {
            _inputService.OnSprintKeyPressed -= SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed -= JumpAnimation;
            _inputService.OnDogdeKeyPressed -= DodgeAnimation;
           // _inputService.OnAttackPressed -= AttackAnimation;
        }

        private void Update()
        {
            var dir = _inputService.MoveDirection;
            _animator.SetFloat("InputX", dir.x, _smoothBlend, Time.deltaTime);
            _animator.SetFloat("InputY", dir.z, _smoothBlend, Time.deltaTime);
        }

        private void SprintAndRunAnimation()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Sprint)
            {
                _animator.SetBool("IsSprint", true);
            }
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Run)
            {
                _animator.SetBool("IsSprint", false);
            }
        }

        private void JumpAnimation()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Jumping)
            {
                _animator.Play("Jump forward");
            }
        }

        private void DodgeAnimation()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Dodge)
            {
                _animator.Play("Dodge");
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
    }
}