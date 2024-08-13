using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboAnimationService : MonoBehaviour
{
    [SerializeField] private RectTransform _keys;

    [Header("KeysNeedToPress")]
    [SerializeField] private GameObject _keyboard;
    [SerializeField] private GameObject _mouse;

    [Header("Animation Components")]
    [SerializeField] private CanvasGroup _canvasGroup;

    private TMP_Text _text;
    private List<GameObject> _mouseButtons;

    public void InstantiateKeys()
    {
        _keyboard = Instantiate(_keyboard, _keys);
        _text = _keyboard.GetComponentInChildren<TMP_Text>();

        _mouse = Instantiate(_mouse, _keys);
        _mouseButtons = GetChilds(_mouse);
    }

    private List<GameObject> GetChilds(GameObject key)
    {
        List<GameObject> childs = new List<GameObject>();

        for (int index = 0; index < key.transform.childCount; index++)
        {
            childs.Add(key.transform.GetChild(index).gameObject);
        }

        return childs;
    }

    public void ManageAnimation(GameObject currentIndicator, GameObject nextIndicator, string nextKey, bool isKeyboard)
    {
        HideOtherKeys();

        currentIndicator.SetActive(true);
        var currentKeyChilds = GetChilds(currentIndicator);
        SwitchBackground(currentKeyChilds, true);

        if (nextIndicator == null) return;

        AnimateKey(nextIndicator, nextKey, isKeyboard);
    }

    public void SetStartState(List<GameObject> keys, string nextButton, bool isKeyboard)
    {
        foreach (var key in keys)
        {
            SwitchBackground(GetChilds(key), false);
            key.SetActive(true);
        }

        AnimateKey(keys[0], nextButton, isKeyboard);
    }

    // Переключает клавишу комбо между уже нажатой и ещё не нажатой
    public void SwitchBackground(List<GameObject> images, bool isPressed)
    {
        images[1].gameObject.SetActive(!isPressed);
        images[2].gameObject.SetActive(isPressed);
    }

    // Анимация показа клавиши для продолжения комбо
    public void AnimateKey(GameObject key, string nextButton, bool isKeyboard)
    {
        GameObject nextKey;

        if (isKeyboard)
        {
            nextKey = _keyboard;
            _text.text = "CTRL";
        }
        else
        {
            nextKey = _mouse;

            foreach(var button in _mouseButtons)
            {
                button.SetActive(false);
            }
            _mouseButtons[0].SetActive(true);
        }

        key.SetActive(false);
        nextKey.SetActive(true);
        nextKey.transform.position = key.transform.position;
    }

    // Скрывает все клавиши продолждения комбо
    private void HideOtherKeys()
    {
        _keyboard.SetActive(false);
        _mouse.SetActive(false);
    }

    // Анимация прерванного комбо
    public void AnimateCancelCombo()
    {
        HideOtherKeys();
        _canvasGroup.alpha = 0.15f;
    }

    // Анимация законченного комбо
    public void AnimateFinishCombo()
    {
        _canvasGroup.alpha = 1f;
    }

    public void FadeOff()
    {
        _canvasGroup.alpha = 1f;
    }
}