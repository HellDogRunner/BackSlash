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

        private bool _runState;

        public Action OnStateChanged;

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
            _inputService.OnDirectionChanged += WalkAnimation;
            _inputService.OnSprintKeyPressed += RunAnimation;
        }

        private void OnDestroy()
        {
            _inputService.OnDirectionChanged -= WalkAnimation;
            _inputService.OnSprintKeyPressed -= RunAnimation;
        }

        private void WalkAnimation(Vector3 direction) 
        {
  
            if (direction != Vector3.zero)
            {
                if (_runState)
                {
                    Animator.SetBool("IsRun", true);
                }
                else
                Animator.SetBool("IsWalk", true);
            }
            else 
            if (direction == Vector3.zero)
            {
                Animator.SetBool("IsWalk", false);
                Animator.SetBool("IsRun", false);
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

        private void RunAnimation(bool runState) 
        {
            _runState = runState;
            if (runState)
            {
                Animator.SetBool("IsRun", true);
            }
            else Animator.SetBool("IsRun", false);

        }
    }
}