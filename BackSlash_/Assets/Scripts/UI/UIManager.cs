using Cinemachine;
using Scripts.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook _camera;

        [Header("Canvases")]
        [SerializeField] private Canvas _pauseCanvas;
        [SerializeField] private Canvas _settingsCanvas;
        [SerializeField] private Canvas _HUDCanvas;
        [SerializeField] private Canvas _currentCanvas;

        [Header("First Selected Buttons")]
        [SerializeField] private GameObject _pauseButton;
        [SerializeField] private GameObject _settingsButton;

        [Header("")]
        [SerializeField] private List<Canvas> _canvasList;

        private bool _isMenuActive;

        private UIController _uiController;
        private InputController _inputController;
        private PauseWindowAnimation _pauseAnimation;

        public List<Canvas> CanvasList => _canvasList;

        [Inject]
        private void Construct(UIController uiController, InputController inputController, PauseWindowAnimation pauseWindowAnimation)
        {
            _uiController = uiController;
            _inputController = inputController;
            _pauseAnimation = pauseWindowAnimation;
        }

        private void Awake()
        {
            _isMenuActive = false;
            _uiController.OnEscapeKeyPressed += PauseMenu;
            _pauseAnimation.OnAnimationShow += AnimationShow;
            _pauseAnimation.OnAnimationHide += AnimationHide;
        }

        private void LateUpdate()
        {
            if (_isMenuActive)
            {
                _camera.m_XAxis.m_InputAxisValue = 0f;
                _camera.m_YAxis.m_InputAxisValue = 0f;
            }
        }

        public void PauseMenu()
        {
            if (_isMenuActive)
            {
                _isMenuActive = false;
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                HideWindow(_currentCanvas);
            }
            else
            {
                _isMenuActive = true;
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.Confined;
                ShowWindow(_pauseCanvas);
            }
            Cursor.visible = _isMenuActive;
            _inputController.enabled = !_isMenuActive;
            _HUDCanvas.gameObject.SetActive(!_isMenuActive);
        }

        public void HideWindow(Canvas canvas)
        {
            _pauseAnimation.HideAnimation(canvas);
        }

        public void ShowWindow(Canvas canvas)
        {
            EventSystem.current.SetSelectedGameObject(_pauseButton);
            _pauseAnimation.ShowAnimation(canvas);
        }

        private void AnimationHide(Canvas canvas)
        {
            canvas.gameObject.SetActive(false);
        }

        private void AnimationShow(Canvas canvas)
        {
            canvas.gameObject.SetActive(true);
            _currentCanvas = canvas;
        }

        private void OnDestroy()
        {
            _uiController.OnEscapeKeyPressed -= PauseMenu;
            _pauseAnimation.OnAnimationShow -= AnimationShow;
            _pauseAnimation.OnAnimationHide -= AnimationHide;

        }
    }
}