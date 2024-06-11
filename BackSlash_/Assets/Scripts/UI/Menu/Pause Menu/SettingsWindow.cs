using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class SettingsWindow : BasePauseWindow
    {
        [Header("Buttons")]
        [SerializeField] private Button _back;

        protected override void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);

            _back.onClick.AddListener(BackClick);
        }

        protected override void OnDisable()
        {
            _back.onClick.RemoveListener(BackClick);
        }

        protected override void PauseMenu()
        {
            if (_settingsMenuCanvas.enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = !_settingsMenuCanvas.enabled;

                _settingsMenuCanvas.gameObject.SetActive(!_settingsMenuCanvas.enabled);
            }
        }

        private void BackClick()
        {
            _settingsMenuCanvas.gameObject.SetActive(false);
            _pauseMenuCanvas.gameObject.SetActive(true);
        }
    }
}
