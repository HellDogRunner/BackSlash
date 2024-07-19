using UnityEngine;
using UnityEngine.UI;

public class BasicTab : MonoBehaviour
{
    [SerializeField] protected Image _selectedImage;

    protected virtual void OnEnable()
    {
        _selectedImage.enabled = true;
    }

    protected virtual void OnDisable()
    {
        _selectedImage.enabled = false;
    }
}
