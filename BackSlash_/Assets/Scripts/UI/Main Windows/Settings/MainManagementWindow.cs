using UnityEngine;
using UnityEngine.UI;


namespace RedMoonGames.Window
{
    public class MainManagementWindow : MainBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _managementHandler;
        [SerializeField] private WindowHandler _settingsHandler;

        [Header("Navigation Keys")]
        [SerializeField] private Button _back;

        private void Awake()
        {
            _uIController.OnBackKeyPressed += Back;

            _back.onClick.AddListener(() => SwitchWindows(_managementHandler, _settingsHandler));
        }

        private void Back()
        {
            SwitchWindows(_managementHandler, _settingsHandler);
        }

        private void OnDestroy()
        {
            _uIController.OnBackKeyPressed -= Back;

            _back.onClick.RemoveAllListeners();
        }
    }
}
