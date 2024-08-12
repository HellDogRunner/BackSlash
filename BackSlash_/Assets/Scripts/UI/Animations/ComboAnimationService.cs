using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboAnimationService : MonoBehaviour
{
    [Header("Keyboard")]
    [SerializeField] private GameObject _keyboard;
    [SerializeField] private TMP_Text _text;
    [Header("Mouse")]
    [SerializeField] private GameObject _mouse;
    [SerializeField] private Image _left;
    [SerializeField] private Image _right;
    [SerializeField] private Image _wheel;

    private List<GameObject> GetChilds(GameObject key)
    {
        List<GameObject> images = new List<GameObject>();

        for (int t = 0; t < key.transform.childCount; t++)
        {
            images.Add(key.transform.GetChild(t).gameObject);
        }

        return images;
    }

    public void ManageAnimation(GameObject currentKey, GameObject nextKey, bool isKeyboard)
    {
        HideOtherKeys();

        currentKey.SetActive(true);
        var currentKeyChilds = GetChilds(currentKey);
        SwitchBackground(currentKeyChilds, true);

        if (nextKey == null) return;

        nextKey.SetActive(false);
        var nextKeyChilds = GetChilds(nextKey);
        AnimateKey(nextKeyChilds[0], isKeyboard);
    }

    public void SwitchBackground(List<GameObject> images, bool isPressed)
    {
        images[1].gameObject.SetActive(!isPressed);
        images[2].gameObject.SetActive(isPressed);
    }

    public void SetStartState(List<GameObject> keys, bool isKeyboard)
    {
        foreach (var key in keys)
        {
            SwitchBackground(GetChilds(key), false);
            key.SetActive(true);
        }

        keys[0].SetActive(false);
        AnimateKey(keys[0], isKeyboard);
    }

    public void AnimateKey(GameObject key, bool isKeyboard)
    {
        GameObject nextKey;

        if (isKeyboard)
        {
            nextKey = _keyboard;
        }
        else
        {
            nextKey = _mouse;
        }

        nextKey.SetActive(true);
        nextKey.transform.position = key.transform.position;
    }

    private void HideOtherKeys()
    {
        _keyboard.SetActive(false);
        _mouse.SetActive(false);
    }

    public void AnimateCancelCombo()
    {
        HideOtherKeys();
    }

    public void AnimateFinishCombo()
    {

    }
}