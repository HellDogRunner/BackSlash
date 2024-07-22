using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class StartSettingsWindow : StartBasicWindow
    {
        [SerializeField] private WindowHandler _settingsHandler;
        [SerializeField] private WindowHandler _startHandler;

        [Header("Buttons")]
        [SerializeField] private Button _back;

        private void OnEnable()
        {
            _animationController.ShowWindowAnimation(_canvasGroup);

            _back.Select();
            _back.onClick.AddListener(BackClick);
        }
        private void OnDisable()
        {
            _back.onClick.RemoveListener(BackClick);
        }

        private void BackClick()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _settingsHandler);
            _windowsManager.OpenWindow(_startHandler);
        }
    }
}