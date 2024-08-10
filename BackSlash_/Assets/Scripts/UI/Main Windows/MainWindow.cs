using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class MainWindow : MainBasicWindow
    {
        [Header("Handlers")]
        [SerializeField] private WindowHandler _mainHandler;
        [SerializeField] private WindowHandler _settingsHandler;
        [SerializeField] private WindowHandler _startHandler;

        [Header("Buttons")]
        [SerializeField] private Button _start;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;

        [Header("Navigation Keys")]
        [SerializeField] private Button _back;

        private void Awake()
        {
            _start.Select();

            _uIController.OnEscapeKeyPressed += Back;

            _start.onClick.AddListener(StartClick);
            _settings.onClick.AddListener(() => SwitchWindows(_mainHandler, _settingsHandler));
            _exit.onClick.AddListener(ExitClick);
        }

        private void StartClick()
        {
            Cursor.visible = false;
            _animationService.HideWindowAnimation(_canvasGroup, _mainHandler, null);
            _sceneTransition.SwichToScene("FirstLocation");
        }

        private void Back()
        {
            _animationService.HideWindowAnimation(_canvasGroup, _mainHandler, _startHandler);
        }

        private void ExitClick()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            _uIController.OnEscapeKeyPressed -= Back;

            _start.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}