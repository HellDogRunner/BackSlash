using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectButtonOnHover : MonoBehaviour, IPointerEnterHandler
{
    private Button _button;

    private void Start()
    {
      TryGetComponent<Button>(out _button);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
        _button.Select();
    }
}
