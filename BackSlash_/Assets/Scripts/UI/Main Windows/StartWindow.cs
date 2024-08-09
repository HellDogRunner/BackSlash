using UnityEngine;

namespace RedMoonGames.Window
{
    public class StartWindow : MainBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _startHandler;
        [SerializeField] private WindowHandler _mainHandler;

        private void Awake()
        {
            _uIController.OnAnyKeyboardKeyPressed += OpenMainMenu;
        }

        private void OpenMainMenu()
        {
            _animationService.HideWindowAnimation(_canvasGroup, _startHandler, _mainHandler);
        }

        private void OnDestroy()
        {
            _uIController.OnAnyKeyboardKeyPressed -= OpenMainMenu;
        }
    }
} 