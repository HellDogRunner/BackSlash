using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class SettingsStartWindow : BaseStartWindow
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

        private void BackClick()
        {
            _settingsCanvas.gameObject.SetActive(false);
            _startMenuCanvas.gameObject.SetActive(true);
        }
    }
}
