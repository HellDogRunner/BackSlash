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

        public Action OnStateChanged;

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
            _inputService.OnDirectionChanged += RunAnimation;
        }

        private void OnDestroy()
        {
            _inputService.OnDirectionChanged -= RunAnimation;
        }

        private void RunAnimation(Vector3 direction) 
        {
            if (direction != Vector3.zero)
            {
                Animator.SetBool("IsWalk", true);
            }
            else
            if (direction == Vector3.zero)
            {
                Animator.SetBool("IsWalk", false);
            }
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
    }
}