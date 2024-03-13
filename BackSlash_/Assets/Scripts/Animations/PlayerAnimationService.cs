using Scripts.Player;
using System;
using UnityEngine;
using Zenject;

namespace Scripts.Animations
{
    public class PlayerAnimationService : MonoBehaviour
    {
        [SerializeField] private Animator Animator;
        [SerializeField] private float smoothBlend;

        private InputService _inputService;

        public event Action OnAttack;

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
            _inputService.OnSprintKeyPressed += SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed += JumpAnimation;
            _inputService.OnLightAttackPressed += AttackAnimation;
            _inputService.OnDogdeKeyPressed += DodgeAnimation;
        }

        private void OnDestroy()
        {
            _inputService.OnSprintKeyPressed -= SprintAndRunAnimation;
            _inputService.OnJumpKeyPressed -= JumpAnimation;
            _inputService.OnLightAttackPressed -= AttackAnimation;
            _inputService.OnDogdeKeyPressed -= DodgeAnimation;
        }

        private void Update()
        {
            var dir = _inputService.MoveDirection;
            Animator.SetFloat("InputX", dir.x, smoothBlend, Time.deltaTime);
            Animator.SetFloat("InputY", dir.z, smoothBlend, Time.deltaTime);
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

        private void JumpAnimation()
        {
            if (_inputService.StateContainer.State == PlayerState.EPlayerState.Jumping)
            {
                Animator.Play("Jump forward");
            }
        }

        private void DodgeAnimation()
        {
            if (_inputService.StateContainer.State == PlayerState.EPlayerState.Dodge)
            {
                Animator.Play("Dodge");
            }
        }

        private void AttackAnimation()
        {
            Animator.Play("LightAttack");
        }
    }
}