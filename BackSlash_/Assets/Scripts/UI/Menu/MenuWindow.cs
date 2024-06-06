using Cinemachine;
using Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Menu
{
    public class MenuWindow : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;

        [Header("Canvas")]
        [SerializeField] private Canvas _pauseMenuCanvas;
        [SerializeField] private Canvas _settingsCanvas;

        [Header("Buttons")]
        [SerializeField] private Button _continue;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;

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

        private void OnDestroy()
        {
            _inputService.OnMenuKeyPressed -= PauseMenu;
        }

        private void LateUpdate()
        {
            if (_isMenuActive)
            {
                _cinemachineFreeLook.m_XAxis.m_InputAxisValue = 0f;
                _cinemachineFreeLook.m_YAxis.m_InputAxisValue = 0f;
            }
        }

        private void PauseMenu()
        {
            if (_isMenuActive)
            {
                _isMenuActive = false;
            }
            else
            {
                _isMenuActive = true;

            }
            SwichMenuWindow(_isMenuActive);
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
            _pauseMenuCanvas.gameObject.SetActive(_swichWindow);
        }

        private void ContinueClick()
        {
            PauseMenu();
        }

        private void SettingsClick()
        {
            _pauseMenuCanvas.gameObject.SetActive(false);
            _settingsCanvas.gameObject.SetActive(true);
        }

        private void ExitClick()
        {
            Debug.Log("Exit");
            Application.Quit();
        }
    }
}
