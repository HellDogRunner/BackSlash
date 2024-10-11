using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private HUDAnimationService _animationService;

    public void SwitchOverlay(int fade = 0)
    {
        _animationService.Overlay(fade);
    }
}