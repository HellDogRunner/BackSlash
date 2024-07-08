using UnityEngine;

namespace RedMoonGames.Window
{
    public class GameGameplayWindow : GameBasicWindow
    {
        [SerializeField] private WindowHandler _gameplayHandler;

        protected override void HideCurrentWindow()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _gameplayHandler);
        }
    }
}