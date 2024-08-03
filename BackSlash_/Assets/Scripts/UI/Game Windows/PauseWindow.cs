using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class PauseWindow : GameBasicWindow
    {
        [SerializeField] private WindowHandler _pauseHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Buttons")]
        [SerializeField] private Button _continue;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;
        [Header("Navigation Keys")]
        [SerializeField] private Button _close;

        private void Awake()
        {
            _continue.Select();
            _continue.onClick.AddListener(ContinueClick);
            _settings.onClick.AddListener(SettingsClick);
            _exit.onClick.AddListener(ExitClick);
            _close.onClick.AddListener(ContinueClick);
        }

        private void ContinueClick()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _pauseHandler);
            _windowManager.SwitchPause();
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

        protected override void DisablePause()
        {
            _animationController.HideWindowAnimation(_canvasGroup, _pauseHandler);
        }

        protected override void OnDestroy()
        {
            _windowManager.OnUnpausing -= DisablePause;
            _windowManager.OnPausing -= EnablePause;

            _continue.onClick.RemoveListener(ContinueClick);
            _settings.onClick.RemoveListener(SettingsClick);
            _exit.onClick.RemoveListener(ExitClick);
            _close.onClick.RemoveListener(ContinueClick);
        }
    }
}