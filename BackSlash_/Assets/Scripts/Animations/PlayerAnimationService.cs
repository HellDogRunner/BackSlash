using Scripts.Player;
using System;
using UnityEngine;
using Zenject;

namespace Scripts.Animations
{
    public class PlayerAnimationService : MonoBehaviour
    {
        [SerializeField] private Animator Animator;

        private InputService _inputService;

        public event Action OnAttack;

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
            _inputService.OnPlayerIdle += IdleAnimation;
            _inputService.OnPlayerWalking += WalkingAnimation;
            _inputService.OnSprintKeyPressed += SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed += JumpAnimation;
            _inputService.OnAirEnding += InAirDisabler;
            _inputService.OnLightAttackPressed += AttackAnimation;
        }

        private void OnDestroy()
        {
            _inputService.OnPlayerIdle -= IdleAnimation;
            _inputService.OnPlayerWalking -= WalkingAnimation;
            _inputService.OnSprintKeyPressed -= SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed -= JumpAnimation;
            _inputService.OnAirEnding -= InAirDisabler;
            _inputService.OnLightAttackPressed -= AttackAnimation;
        }

        private void IdleAnimation()
        {
            if (_inputService.StateContainer.State == PlayerState.EPlayerState.Idle)
            {
                Animator.SetBool("IsRun", false);
                Animator.SetBool("IsSprint", false);
            }
            else if (_inputService.StateContainer.State == PlayerState.EPlayerState.Run)
            {
                Animator.SetBool("IsRun", true);
            }
        }

        private void InAirDisabler()
        {
            Animator.SetBool("InAir", false);
        }

        private void WalkingAnimation()
        {
            if (_inputService.StateContainer.State == PlayerState.EPlayerState.Walk)
            {
                Animator.SetBool("IsWalk", true);
            }
            else if (_inputService.StateContainer.State != PlayerState.EPlayerState.Walk)
            {
                Animator.SetBool("IsWalk", false);
            }
        }

        private void SprintAndRunAnimation()
        {
            if (_inputService.StateContainer.State == PlayerState.EPlayerState.Sprint)
            {
                Animator.SetBool("IsSprint", true);
            }
            else if (_inputService.StateContainer.State == PlayerState.EPlayerState.Run)
            {
                Animator.SetBool("IsSprint", false);
            }
        }

        private void SiddewayWalking(Vector3 direction)
        {
            if (direction == new Vector3(-1f, 0f, 1f) || direction == new Vector3(1f, 0f, 1f))
            {
                Animator.SetBool("Sideways", true);
                if (direction.x == -1f)
                {
                    Animator.SetFloat("Blend", 0.1f);
                }
                else Animator.SetFloat("Blend", 0.9f);
            }
            else { Animator.SetBool("Sideways", false); }
        }

        private void JumpAnimation()
        {
            if (_inputService.StateContainer.State == PlayerState.EPlayerState.Jump)
            {
                Animator.SetBool("InAir", true);
            }
            else if (_inputService.StateContainer.State != PlayerState.EPlayerState.InAir)
            {
                Animator.SetBool("InAir", false);
            }
        }

        private void AttackAnimation()
        {
            Animator.Play("LightAttack");
        }
    }
}