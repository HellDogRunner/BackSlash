using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
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