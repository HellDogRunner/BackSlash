using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class GameplayWindow : GameBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Navigation Keys")]
        [SerializeField] private Button _back;
        [SerializeField] private Button _close;

        private void Awake()
        {
            _pauseInputs.OnBackKeyPressed += BackButton;
            _back.onClick.AddListener(BackButton);
            if (!_IsMainMenu) _close.onClick.AddListener(_windowService.Unpause);
        }

        private void BackButton() { OpenWindow(_settingsHandler); }

        private void OnDestroy()
        {
            _windowService.OnHideWindow -= DisablePause;
            _pauseInputs.OnBackKeyPressed -= BackButton;

            _back.onClick.RemoveListener(BackButton);
            if (!_IsMainMenu) _close.onClick.RemoveListener(_windowService.Unpause);
        }
    }
}