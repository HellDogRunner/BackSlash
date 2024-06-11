using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public abstract class BaseButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        protected virtual void Awake()
        {
            _button ??= GetComponent<Button>();
        }

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        protected abstract void HandleButtonClick();
    }
}