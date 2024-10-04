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

            _pauseInputs.OnBackKeyPressed += _windowService.Unpause;

            _continue.onClick.AddListener(_windowService.Unpause);
            _settings.onClick.AddListener(() => SwitchWindows(_pauseHandler, _settingsHandler));
            _exit.onClick.AddListener(ExitClick);
            _close.onClick.AddListener(_windowService.Unpause);
        }

        private void ExitClick()
        {
            _windowService.ChangeScene("StartMenu");
            _windowService.Unpause();
        }

        protected override void EnablePause()
        {
            _animationService.ShowWindowAnimation(_canvasGroup);
        }

        private void OnDestroy()
        { 
            _windowService.OnHideWindow -= DisablePause;
            _windowService.OnShowWindow -= EnablePause;

            _pauseInputs.OnBackKeyPressed -= _windowService.Unpause;

            _continue.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
            _close.onClick.RemoveAllListeners();
        }
    }
}