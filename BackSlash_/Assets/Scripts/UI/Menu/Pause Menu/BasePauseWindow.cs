using Cinemachine;
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

        [Header("First Selected Field")]
        [SerializeField] protected GameObject _mainMenuFirst;
        [SerializeField] protected GameObject _settingsMenuFirst;

        protected UIController _uiController;
        protected TargetLock _targetLock;

        protected bool _isMenuActive;

        [Inject]
        protected virtual void Construct(UIController uiController, TargetLock targetLock)
        {
            _uiController = uiController;
            _targetLock = targetLock;

            _isMenuActive = false;

            _uiController.OnEscapeKeyPressed += PauseMenu;
        }

        protected virtual void OnDestroy()
        {
            _uiController.OnEscapeKeyPressed -= PauseMenu;
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
                _isMenuActive = false;

                Cursor.lockState = CursorLockMode.Locked;
                _pauseMenuCanvas.gameObject.SetActive(_isMenuActive);
            }
            else
            {
                _isMenuActive = true;

                Cursor.lockState = CursorLockMode.Confined;
                _pauseMenuCanvas.gameObject.SetActive(_isMenuActive);
            }
            Cursor.visible = _isMenuActive;

            _targetLock.enabled = !_isMenuActive;
        }
    }
}