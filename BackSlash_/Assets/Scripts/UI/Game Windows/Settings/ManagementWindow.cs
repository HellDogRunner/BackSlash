using UnityEngine;
using UnityEngine.UI;


namespace RedMoonGames.Window
{
    public class ManagementWindow : GameBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _managementHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Navigation Keys")]
        [SerializeField] private Button _back;
        [SerializeField] private Button _close;

        private void Awake()
        {
            _pauseInputs.OnBackKeyPressed += Back;

            _back.onClick.AddListener(() => SwitchWindows(_managementHandler, _settingsHandler));
            if (!_IsMainMenu) _close.onClick.AddListener(_windowService.Unpause);
        }

        private void Back()
        {
            SwitchWindows(_managementHandler, _settingsHandler);
        }

        private void OnDestroy()
        {
            _windowService.OnHideWindow -= DisablePause;
            _pauseInputs.OnBackKeyPressed -= Back;

            _back.onClick.RemoveAllListeners();
            if (!_IsMainMenu) _close.onClick.RemoveListener(_windowService.Unpause);
        }
    }
}
