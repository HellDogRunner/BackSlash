using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class PauseWindow : GameBasicWindow
    {
        [Header("Handlers")]
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

            _menuActions.enabled = false;
            _uIController.enabled = true;
            _uIController.OnBackKeyPressed += _windowsController.PausePressed;

            _continue.onClick.AddListener(_windowsController.PausePressed);
            _settings.onClick.AddListener(() => SwitchWindows(_pauseHandler, _settingsHandler));
            _exit.onClick.AddListener(ExitClick);
            _close.onClick.AddListener(_windowsController.PausePressed);
        }

        private void ExitClick()
        {
            Cursor.visible = false;
            _sceneTransition.SwichToScene("StartMenu");
        }

        protected override void EnablePause()
        {
            _animationService.ShowWindowAnimation(_canvasGroup);
        }

        private void OnDestroy()
        { 
            _windowsController.OnUnpausing -= DisablePause;
            _windowsController.OnPausing -= EnablePause;

            _uIController.OnBackKeyPressed -= _windowsController.PausePressed;

            _continue.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
            _close.onClick.RemoveAllListeners();
        }
    }
}