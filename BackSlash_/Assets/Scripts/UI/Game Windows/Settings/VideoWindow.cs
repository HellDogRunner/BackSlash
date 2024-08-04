using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class VideoWindow : GameBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _videoHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Navigation Keys")]
        [SerializeField] private Button _back;
        [SerializeField] private Button _close;

        private void Awake()
        {
            _controller.OnBackKeyPressed += Back;

            _back.onClick.AddListener(() => SwitchWindows(_videoHandler, _settingsHandler));
            _close.onClick.AddListener(_windowManager.SwitchPause);
        }

        private void Back()
        {
            SwitchWindows(_videoHandler, _settingsHandler);
        }

        private void OnDestroy()
        {
            _windowManager.OnUnpausing -= DisablePause;

            _controller.OnBackKeyPressed -= Back;

            _back.onClick.RemoveAllListeners();
            _close.onClick.RemoveAllListeners();
        }
    }
}