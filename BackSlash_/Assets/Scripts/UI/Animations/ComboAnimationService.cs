using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboAnimationService : MonoBehaviour
{
    [Header("Keyboard")]
    [SerializeField] private GameObject _keyKB;
    [SerializeField] private TMP_Text _textKB;
    [Header("Mouse")]
    [SerializeField] private GameObject _mouse;
    [SerializeField] private Image _left;
    [SerializeField] private Image _right;
    [SerializeField] private Image _wheel;

    private List<Transform> GetChilds(GameObject key)
    {
        List<Transform> images = new List<Transform>();

        for (int t = 0; t < key.transform.childCount; t++)
        {
            images.Add(key.transform.GetChild(t));
        }
        return images;
    }

    public void ManageAnimation(GameObject key, bool isKeyboard)
    {
        var keyChilds = GetChilds(key);
        SwitchBackground(keyChilds, true);

        if (isKeyboard)
        {
            KeyboardAnimation(keyChilds[0]);
        }
        else
        {
            MouseAnimation(keyChilds[0]);
        }
    }

    public void SwitchBackground(List<Transform> images, bool isPressed)
    {
        images[1].gameObject.SetActive(!isPressed);
        images[2].gameObject.SetActive(isPressed);
    }

    public void SetDefaultState(List<GameObject> keys)
    {
        foreach (var key in keys)
        {
            SwitchBackground(GetChilds(key), false);
        }
        KeyboardAnimation(keys[0].transform);
    }

    public void KeyboardAnimation(Transform key)
    {
        _keyKB.transform.position = key.position;
    }

    public void MouseAnimation(Transform key)
    {
        _mouse.transform.position = key.position;
    }

    public void AnimateFinishCombo()
    {
        Debug.Log("fin");
    }
}