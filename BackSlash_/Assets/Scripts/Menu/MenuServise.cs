using Scripts.Player;
using UnityEngine;
using Zenject;

namespace Scripts.Menu
{
    public class MenuServise : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private GameObject _player;
        [Header("Debug")]
        [SerializeField] private bool _isMenuActive;
        [SerializeField] private GameObject _pauseMenuWindow;

        private Transform _camera;
        private InputController _inputService;
        private TargetLock _targetLock;

        private float _mouseX;
        private float _mouseY;

        [Inject]
        private void Construct(InputController inputService)
        {
            _inputService = inputService;

            _inputService.OnMenuKeyPressed += PauseMenu;

            _pauseMenuWindow = gameObject.transform.GetChild(0).gameObject;
            _targetLock = _player.GetComponent<TargetLock>();

            _isMenuActive = false;
        }

        private void OnDestroy()
        {
            _inputService.OnMenuKeyPressed -= PauseMenu;
        }

        private void PauseMenu()
        {
            if (_isMenuActive)
            {
                _isMenuActive = false;
                _targetLock.MenuSwich(_isMenuActive);
            }
            else
            {
                _isMenuActive = true;
                _targetLock.MenuSwich(_isMenuActive);
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

            _pauseMenuWindow.SetActive(_isMenuActive);
        }
    }
}
