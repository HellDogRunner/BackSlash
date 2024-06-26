using Cinemachine;
using Scripts.Player;
using UnityEngine;
using Zenject;

namespace Scripts.UI
{
    public abstract class BasePauseWindow : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] protected CinemachineFreeLook _cinemachineFreeLook;

        [Header("Canvas")]
        [SerializeField] protected Canvas _pauseMenuCanvas;
        [SerializeField] protected Canvas _settingsMenuCanvas;
        [SerializeField] private Canvas _gameplayHUDCanvas;

        [Header("First Selected Field")]
        [SerializeField] protected GameObject _mainMenuFirst;
        [SerializeField] protected GameObject _settingsMenuFirst;

        protected UIController _uiController;
        protected InputController _inputController;

        protected bool _isMenuActive;

        [Inject]
        protected virtual void Construct(UIController uiController, InputController inputController)
        {
            _uiController = uiController;
            _inputController = inputController;

            _isMenuActive = false;

            _uiController.OnEscapeKeyPressed += PauseMenu;
        }

        protected abstract void OnEnable();

        protected abstract void OnDisable();

        protected virtual void LateUpdate()
        {
            if (_isMenuActive)
            {
                _cinemachineFreeLook.m_XAxis.m_InputAxisValue = 0f;
                _cinemachineFreeLook.m_YAxis.m_InputAxisValue = 0f;
            }
        }

        protected virtual void PauseMenu()
        {
            if (_isMenuActive)
            {
                Time.timeScale = 1f;
                
                _gameplayHUDCanvas.gameObject.SetActive(_isMenuActive);
                
                _isMenuActive = false;

                Cursor.lockState = CursorLockMode.Locked;
                _pauseMenuCanvas.gameObject.SetActive(_isMenuActive);
            }
            else
            {
                Time.timeScale = 0f;

                _gameplayHUDCanvas.gameObject.SetActive(_isMenuActive);

                _isMenuActive = true;

                Cursor.lockState = CursorLockMode.Confined;
                _pauseMenuCanvas.gameObject.SetActive(_isMenuActive);
            }

            Cursor.visible = _isMenuActive;
            _inputController.enabled = !_isMenuActive;
        }

        protected virtual void OnDestroy()
        {
            _uiController.OnEscapeKeyPressed -= PauseMenu;
        }
    }
}