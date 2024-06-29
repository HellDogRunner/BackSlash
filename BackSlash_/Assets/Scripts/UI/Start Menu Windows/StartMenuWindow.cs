using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class StartMenuWindow : StartBasicWindow
    {
        [SerializeField] private WindowHandler _startHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Buttons")]
        [SerializeField] private Button _start;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;

        private void OnEnable()
        {
            _animationController.ShowWindowAnimation(_canvasGroup);

            _start.Select();
            _start.onClick.AddListener(StartClick);
            _settings.onClick.AddListener(SettingsClick);
            _exit.onClick.AddListener(ExitClick);
        }

        private void OnDisable()
        {
            _start.onClick.RemoveListener(StartClick);
            _settings.onClick.RemoveListener(SettingsClick);
            _exit.onClick.RemoveListener(ExitClick);
        }

        private void StartClick()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _startHandler);
            _sceneTransition.SwichToScene("FirstLocation");
        }

        private void SettingsClick()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _startHandler);
            _windowsManager.OpenWindow(_settingsHandler);
        }

        private void ExitClick()
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }
}