using UnityEngine.UI;
using Zenject;
using UnityEngine;
using Scripts.Player;

namespace Scripts.Menu
{
    public class SettingsWindow : MonoBehaviour
    {
        [Header("Canvas")] 
        [SerializeField] private Canvas _settingsCanvas;
        [SerializeField] private Canvas _pauseMenuCanvas;

        [Header("Buttons")]
        [SerializeField] private Button _back;

        private InputController _inputService;
        private TargetLock _targetLock;

        private bool _isMenuActive;

        [Inject]
        private void Construct(InputController inputService, TargetLock targetLock)
        {
            _inputService = inputService;
            _targetLock = targetLock;

            _inputService.OnMenuKeyPressed += PauseMenu;

            _isMenuActive = false;
        }

        private void OnEnable()
        {
            _back.onClick.AddListener(BackClick);
        }

        private void OnDisable()
        {
            _back.onClick.RemoveListener(BackClick);
        }

        private void OnDestroy()
        {
            _inputService.OnMenuKeyPressed -= PauseMenu;
        }

        private void BackClick()
        {
            _isMenuActive = false;
            _settingsCanvas.gameObject.SetActive(false);
            _pauseMenuCanvas.gameObject.SetActive(true);
        }

        private void PauseMenu()
        {
            if (_isMenuActive)
            {
                _isMenuActive = false;
                SwichMenuWindow(_isMenuActive);
            }
        }
        private void SwichMenuWindow(bool _swichWindow)
        {
            if (_swichWindow)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            Cursor.visible = _swichWindow;

            _targetLock.enabled = !_swichWindow;
        }
    }
}
