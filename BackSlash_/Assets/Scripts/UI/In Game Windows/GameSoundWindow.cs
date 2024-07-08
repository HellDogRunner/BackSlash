using UnityEngine;

namespace RedMoonGames.Window
{
    public class GameSoundWindow : GameBasicWindow
    {
        [SerializeField] private WindowHandler _soundHandler;

        protected override void HideCurrentWindow()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _soundHandler);
        }
    }
}