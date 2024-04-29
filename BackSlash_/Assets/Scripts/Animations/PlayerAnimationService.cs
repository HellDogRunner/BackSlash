using Scripts.Player;
using System;
using UnityEngine;
using Zenject;
using UnityEngine.Animations.Rigging;

namespace Scripts.Animations
{
    public class PlayerAnimationService : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _smoothBlend;

        [SerializeField] private float _aimDuration;

        private InputService _inputService;

        private AnimatorOverrideController _overrides;

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;

            _inputService.OnSprintKeyPressed += SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed += JumpAnimation;
            _inputService.OnDogdeKeyPressed += DodgeAnimation;
            _inputService.OnSprintKeyPressed += SprintAndRunAnimation;

            _overrides = _animator.runtimeAnimatorController as AnimatorOverrideController;
        }

        private void OnDestroy()
        {
            _inputService.OnSprintKeyPressed -= SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed -= JumpAnimation;
            _inputService.OnDogdeKeyPressed -= DodgeAnimation;
        }

        private void Update()
        {
            var dir = _inputService.MoveDirection;
            _animator.SetFloat("InputX", dir.x, _smoothBlend, Time.deltaTime);
            _animator.SetFloat("InputY", dir.z, _smoothBlend, Time.deltaTime);

            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Attack)
            {
            }
            else if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Idle)
            {
                 //aimLayer.weight -= Time.deltaTime / aimDuration;
            }
        }

        public void ShowWeapon(RaycastWeapon weapon)
        {
            _animator.SetLayerWeight(1, 1f);
            _overrides["weapon_anim_none"] = weapon.WeaponAnimation;
        }

        public void HideWeapon()
        {
            _animator.SetLayerWeight(1, 0f);
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

        private void AttackAnimation()
        {
           // Animator.Play("LightAttack");
        }
    }
}