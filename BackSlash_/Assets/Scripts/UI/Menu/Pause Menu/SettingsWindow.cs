using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private UIManager _manager;
        [SerializeField] private Canvas _canvas;

        [Header("Buttons")]
        [SerializeField] private Button _back;

        private void OnEnable()
        {
            _back.onClick.AddListener(BackClick);
        }

        private void OnDisable()
        {
            _back.onClick.RemoveListener(BackClick);
        }

        private void BackClick()
        {
            _manager.HideWindow(_canvas);
            var pauseWindow = _manager.CanvasList.Find(i => i.gameObject.name == "Pause Menu");
            _manager.ShowWindow(pauseWindow);
        }
    }
}
