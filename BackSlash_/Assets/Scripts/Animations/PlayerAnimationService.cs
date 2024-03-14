using Scripts.Player;
using System;
using UnityEngine;
using Zenject;
using UnityEngine.Animations.Rigging;

namespace Scripts.Animations
{
    public class PlayerAnimationService : MonoBehaviour
    {
        [SerializeField] private Animator Animator;
        [SerializeField] private float smoothBlend;
        [SerializeField] private Rig aimLayer;

        [SerializeField] private float aimDuration;

        private InputService _inputService;

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
            _inputService.OnSprintKeyPressed += SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed += JumpAnimation;
           // _inputService.OnLightAttackPressed += AttackAnimation;
            _inputService.OnDogdeKeyPressed += DodgeAnimation;
        }

        private void OnDestroy()
        {
            _inputService.OnSprintKeyPressed -= SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed -= JumpAnimation;
           // _inputService.OnLightAttackPressed -= AttackAnimation;
            _inputService.OnDogdeKeyPressed -= DodgeAnimation;
        }

        private void Update()
        {
            var dir = _inputService.MoveDirection;
            Animator.SetFloat("InputX", dir.x, smoothBlend, Time.deltaTime);
            Animator.SetFloat("InputY", dir.z, smoothBlend, Time.deltaTime);

            if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Attack)
            {
                aimLayer.weight += Time.deltaTime / aimDuration;
            }
            else if (_inputService.WeaponStateContainer.State == WeaponState.EWeaponState.Idle)
            {
                aimLayer.weight -= Time.deltaTime / aimDuration;
            }
        }

        private void SprintAndRunAnimation()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Sprint)
            {
                Animator.SetBool("IsSprint", true);
            }
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Run)
            {
                Animator.SetBool("IsSprint", false);
            }
        }

        private void JumpAnimation()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Jumping)
            {
                Animator.Play("Jump forward");
            }
        }

        private void DodgeAnimation()
        {
            if (_inputService.PlayerStateContainer.State == PlayerState.EPlayerState.Dodge)
            {
                Animator.Play("Dodge");
            }
        }

        private void AttackAnimation()
        {
           // Animator.Play("LightAttack");
        }
    }
}