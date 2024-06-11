using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{

    public class MainStartWindow : BaseStartWindow
    {
        [Header("Buttons")]
        [SerializeField] private Button _start;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;

        protected override void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
            _start.onClick.AddListener(StartClick);
            _settings.onClick.AddListener(SettingsClick);
            _exit.onClick.AddListener(ExitClick);
        }

        protected override void OnDisable()
        {
            _start.onClick.RemoveListener(StartClick);
            _settings.onClick.RemoveListener(SettingsClick);
            _exit.onClick.RemoveListener(ExitClick);
        }

        private void StartClick()
        {
            SceneTransition.SwichToScene("FirstLocation");
        }

        private void SettingsClick()
        {
            _startMenuCanvas.gameObject.SetActive(false);
            _settingsCanvas.gameObject.SetActive(true);
        }

        private void ExitClick()
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }
}

