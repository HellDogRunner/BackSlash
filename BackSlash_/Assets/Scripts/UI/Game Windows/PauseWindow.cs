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

            _animationService.ShowWindowAnimation(_canvasGroup);

            _uIController.OnBackKeyPressed += _windowsController.SwitchPause;

            _continue.onClick.AddListener(_windowsController.SwitchPause);
            _settings.onClick.AddListener(() => SwitchWindows(_pauseHandler, _settingsHandler));
            _exit.onClick.AddListener(ExitClick);
            _close.onClick.AddListener(_windowsController.SwitchPause);
        }

        private void ExitClick()
        {
            Cursor.visible = false;
            _sceneTransition.SwichToScene("StartMenu");
        }

        private void OnDestroy()
        { 
            _windowsController.OnUnpausing -= DisablePause;

            _uIController.OnBackKeyPressed -= _windowsController.SwitchPause;

            _continue.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
            _close.onClick.RemoveAllListeners();
        }
    }
}