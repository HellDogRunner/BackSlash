using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class MainSettingsWindow : MainBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _settingsHandler;
        [SerializeField] private WindowHandler _mainHandler;
        //[SerializeField] private WindowHandler _gameplayHandler;
        //[SerializeField] private WindowHandler _audioHandler;
        //[SerializeField] private WindowHandler _videoHandler;
        //[SerializeField] private WindowHandler _managementHandler;

        [Header("Buttons")]
        [SerializeField] private Button _gameplay;
        [SerializeField] private Button _audio;
        [SerializeField] private Button _video;
        [SerializeField] private Button _management;

        [Header("Navigation Keys")]
        [SerializeField] private Button _back;

        private void Awake()
        {
            _gameplay.Select();

            _uIController.OnBackKeyPressed += Back;

            //_gameplay.onClick.AddListener(() => SwitchWindows(_settingsHandler, _gameplayHandler));
            //_audio.onClick.AddListener(() => SwitchWindows(_settingsHandler, _audioHandler));
            //_video.onClick.AddListener(() => SwitchWindows(_settingsHandler, _videoHandler));
            //_management.onClick.AddListener(() => SwitchWindows(_settingsHandler, _managementHandler));

            _back.onClick.AddListener(() => SwitchWindows(_settingsHandler, _mainHandler));
        }

        private void Back()
        {
            SwitchWindows(_settingsHandler, _mainHandler);
        }

        private void OnDestroy()
        {
            _uIController.OnBackKeyPressed -= Back;

            _gameplay.onClick.RemoveAllListeners();
            _audio.onClick.RemoveAllListeners();
            _video.onClick.RemoveAllListeners();
            _management.onClick.RemoveAllListeners();

            _back.onClick.RemoveAllListeners();
        }
    }
}