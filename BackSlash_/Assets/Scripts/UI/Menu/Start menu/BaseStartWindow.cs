using UnityEngine;
using Zenject;

namespace Scripts.UI
{
    public abstract class BaseStartWindow : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] protected Canvas _startMenuCanvas;
        [SerializeField] protected Canvas _settingsCanvas;

        [Header("First Selected Field")]
        [SerializeField] protected GameObject _mainMenuFirst;
        [SerializeField] protected GameObject _settingsMenuFirst;

        protected UIController _uiController;

        [Inject]
        private void Construct(UIController uiController)
        {
            _uiController = uiController;

            _uiController.OnEscapeKeyPressed += Escape;
        }

        private void OnDestroy()
        {
            _uiController.OnEscapeKeyPressed -= Escape;
        }

        protected abstract void OnEnable();
            
        protected abstract void OnDisable();

        protected virtual void Escape()
        {
            if (_settingsCanvas.enabled)
            {
                _settingsCanvas.gameObject.SetActive(false);
                _startMenuCanvas.gameObject.SetActive(true);
            }
        }
    }
}