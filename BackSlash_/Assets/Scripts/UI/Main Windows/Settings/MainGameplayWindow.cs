using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class MainGameplayWindow : MainBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _gameplayHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Navigation Keys")]
        [SerializeField] private Button _back;

        private void Awake()
        {
            _uIController.OnBackKeyPressed += Back;

            _back.onClick.AddListener(() => SwitchWindows(_gameplayHandler, _settingsHandler));
        }

        private void Back()
        {
            SwitchWindows(_gameplayHandler, _settingsHandler);
        }

        private void OnDestroy()
        {
            _uIController.OnBackKeyPressed -= Back;

            _back.onClick.RemoveAllListeners();
        }
    }
}