using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

        private GameMenuController _menuController;

        [Inject]
        private void Construct(GameMenuController menuController)
        {
            _menuController = menuController;
        }

        private void Awake()
        {
            _continue.Select();

            _pauseInputs.OnBackKeyPressed += _windowService.Unpause;
            _windowService.OnShowWindow += EnablePause;

            _continue.onClick.AddListener(_windowService.Unpause);
            _settings.onClick.AddListener(SettingsButton);
            _exit.onClick.AddListener(ExitButton);
            _close.onClick.AddListener(_windowService.Unpause);
        }

        private void SettingsButton() { SwitchWindows(_pauseHandler, _settingsHandler); }

        private void ExitButton()
        {
            _menuController.ChangeScene("StartMenu");
        }

        private void OnDestroy()
        { 
            _windowService.OnHideWindow -= DisablePause;
            _windowService.OnShowWindow -= EnablePause;

            _pauseInputs.OnBackKeyPressed -= _windowService.Unpause;

            _continue.onClick.RemoveListener(_windowService.Unpause);
            _settings.onClick.RemoveListener(SettingsButton);
            _exit.onClick.RemoveListener(ExitButton);
            _close.onClick.RemoveListener(_windowService.Unpause);
        }
    }
}