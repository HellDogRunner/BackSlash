using UnityEngine;

namespace RedMoonGames.Window
{
    public class GameManagementWindow : GameBasicWindow
    {
        [SerializeField] private WindowHandler _managementHandler;

        protected override void HideCurrentWindow()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _managementHandler);
        }
    }
}
