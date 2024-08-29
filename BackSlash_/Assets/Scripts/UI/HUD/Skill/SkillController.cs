using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] private SkillAnimationService _animationService;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetStartState();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _animationService.AnimateReloading();
        }
    }

    private void SetStartState()
    {
        _animationService.AnimateCooldown();
        //_animationService.AnimateSelection();
    }
}