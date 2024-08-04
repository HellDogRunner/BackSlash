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

            _controller.OnBackKeyPressed += _windowManager.SwitchPause;

            _continue.onClick.AddListener(_windowManager.SwitchPause);
            _settings.onClick.AddListener(() => SwitchWindows(_pauseHandler, _settingsHandler));
            _exit.onClick.AddListener(ExitClick);

            _close.onClick.AddListener(_windowManager.SwitchPause);

            _windowManager.OnPausing += EnablePause;
        }

        private void ExitClick()
        {
            Cursor.visible = false;
            _sceneTransition.SwichToScene("StartMenu");
        }

        private void EnablePause()
        {
            _animationController.ShowWindowAnimation(_canvasGroup);
        }

        private void OnDestroy()
        { 
            _windowManager.OnUnpausing -= DisablePause;
            _windowManager.OnPausing -= EnablePause;

            _controller.OnBackKeyPressed -= _windowManager.SwitchPause;

            _continue.onClick.RemoveListener(_windowManager.SwitchPause);
            _settings.onClick.RemoveAllListeners();
            _exit.onClick.RemoveListener(ExitClick);
            _close.onClick.RemoveListener(_windowManager.SwitchPause);
        }
    }
}