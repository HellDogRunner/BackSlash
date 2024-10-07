using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
    public class MainWindow : GameBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _mainHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Buttons")]
        [SerializeField] private Button _start;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;

        private MainMenuController _menuController;

        [Inject]
        private void Construct(MainMenuController menuController)
        {
            _menuController = menuController;
        }

        private void Awake()
        {
            _start.Select();

            _windowService.OnShowWindow += EnablePause;

            _start.onClick.AddListener(StartButton);
            _settings.onClick.AddListener(SettingsButton);
            _exit.onClick.AddListener(ExitButton);
        }

        private void StartButton() { _menuController.ChangeScene("FirstLocation"); }
        private void SettingsButton() { SwitchWindows(_mainHandler, _settingsHandler); }
        private void ExitButton() { Application.Quit(); }

        private void OnDestroy()
        {
            _windowService.OnHideWindow -= DisablePause;
            _windowService.OnShowWindow -= EnablePause;

            _start.onClick.RemoveListener(StartButton);
            _settings.onClick.RemoveListener(SettingsButton);
            _exit.onClick.RemoveListener(ExitButton);
        }
    }
}