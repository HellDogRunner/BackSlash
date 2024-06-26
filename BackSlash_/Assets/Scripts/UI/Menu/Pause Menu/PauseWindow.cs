using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class PauseWindow : MonoBehaviour
    {
        [SerializeField] private UIManager _manager;
        [SerializeField] private Canvas _canvas;

        [Header("Buttons")]
        [SerializeField] private Button _continue;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;

        private void OnEnable()
        {
            _continue.onClick.AddListener(ContinueClick);
            _settings.onClick.AddListener(SettingsClick);
            _exit.onClick.AddListener(ExitClick);
        }
        private void OnDisable()
        {
            _continue.onClick.RemoveListener(ContinueClick);
            _settings.onClick.RemoveListener(SettingsClick);
            _exit.onClick.RemoveListener(ExitClick);
        }

        private void ContinueClick()
        {
            _manager.PauseMenu();
        }

        private void SettingsClick()
        {
            _manager.HideWindow(_canvas);
            var settingsWindow = _manager.CanvasList.Find(i => i.gameObject.name == "Settings Menu");
            _manager.ShowWindow(settingsWindow);
        }

        private void ExitClick()
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }
}
