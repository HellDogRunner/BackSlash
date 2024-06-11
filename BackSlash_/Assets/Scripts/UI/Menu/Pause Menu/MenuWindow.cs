using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class MenuWindow : BasePauseWindow
    {
        [Header("Buttons")]
        [SerializeField] private Button _continue;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;

        protected override void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_mainMenuFirst);

            _continue.onClick.AddListener(ContinueClick);
            _settings.onClick.AddListener(SettingsClick);
            _exit.onClick.AddListener(ExitClick);
        }
        protected override void OnDisable()
        {
            _continue.onClick.RemoveListener(ContinueClick);
            _settings.onClick.RemoveListener(SettingsClick);
            _exit.onClick.RemoveListener(ExitClick);
        }

        private void ContinueClick()
        {
            PauseMenu();
        }

        private void SettingsClick()
        {
            _pauseMenuCanvas.gameObject.SetActive(false);
            _settingsMenuCanvas.gameObject.SetActive(true);
        }

        private void ExitClick()
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }
}
