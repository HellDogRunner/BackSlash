using UnityEngine;
using UnityEngine.UI;

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

        private void Awake()
        {
            _start.Select();

            _windowService.OnShowWindow += EnablePause;

            _start.onClick.AddListener(StartClick);
            _settings.onClick.AddListener(() => SwitchWindows(_mainHandler, _settingsHandler));
            _exit.onClick.AddListener(ExitClick);
        }

        private void StartClick()
        {
            _windowService.HideWindow();
            _windowService.ChangeScene("FirstLocation");
        }

        private void ExitClick()
        {
            Application.Quit();
        }

        protected override void EnablePause()
        {
            _animationService.ShowWindowAnimation(_canvasGroup);
        }

        private void OnDestroy()
        {
            _windowService.OnHideWindow -= DisablePause;
            _windowService.OnShowWindow -= EnablePause;

            _start.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}