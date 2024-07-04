using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class GamePauseWindow : GameBasicWindow
    {
        [SerializeField] private WindowHandler _pauseHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Buttons")]
        [SerializeField] private Button _continue;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;

        private void OnEnable()
        {
            _continue.Select();
            _continue.onClick.AddListener(ContinueClick);
            _settings.onClick.AddListener(SettingsClick);
            _exit.onClick.AddListener(ExitClick);
        }

        private void OnDisable()
        {
            _continue.onClick.RemoveListener(ContinueClick);
            _settings.onClick.RemoveListener(SettingsClick);
            _exit.onClick.RemoveListener(ExitClick);
        }

        private void ContinueClick()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _pauseHandler);
            _windowManager.PauseSwitch();
        }

        private void SettingsClick()
        {
            _windowManager.CloseWindow(_pauseHandler);
            _windowManager.OpenWindow(_settingsHandler);
        }

        private void ExitClick()
        {
            Cursor.visible = false;
            _sceneTransition.SwichToScene("StartMenu");
        }

        protected override void EnablePause()
        {
            _animationController.ShowWindowAnimation(_canvasGroup);
        }
    }
}