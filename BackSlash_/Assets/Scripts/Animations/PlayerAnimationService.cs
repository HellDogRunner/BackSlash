using Scripts.Player;
using System;
using UnityEngine;
using Zenject;
using UnityEngine.Animations.Rigging;

namespace Scripts.Animations
{
    public class PlayerAnimationService : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float smoothBlend;

        [SerializeField] private float aimDuration;

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

            _overrides = animator.runtimeAnimatorController as AnimatorOverrideController;
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
            animator.SetFloat("InputX", dir.x, smoothBlend, Time.deltaTime);
            animator.SetFloat("InputY", dir.z, smoothBlend, Time.deltaTime);

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
            animator.SetLayerWeight(1, 1f);
            _overrides["weapon_anim_none"] = weapon.WeaponAnimation;
        }

        public void HideWeapon()
        {
            animator.SetLayerWeight(1, 0f);
        }

        private void SprintAndRunAnimation()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Sprint)
            {
                animator.SetBool("IsSprint", true);
            }
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Run)
            {
                animator.SetBool("IsSprint", false);
            }
        }

        private void JumpAnimation()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Jumping)
            {
                animator.Play("Jump forward");
            }
        }

        private void DodgeAnimation()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Dodge)
            {
                animator.Play("Dodge");
            }
        }

        private void AttackAnimation()
        {
           // Animator.Play("LightAttack");
        }
    }
}