using UnityEngine;
using UnityEngine.UI;

public class ImageTransition : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image _image;

    [Header("Parametrs")]
    [SerializeField] private float _fill;

    private bool _isTransitionStart;

    private void Start()
    {
        _fill = 1f;
    }

    private void Update()
    {
        if (_isTransitionStart)
        {
            _fill += Time.deltaTime;
            _image.fillAmount = _fill;
        }
        if (_fill >= 1)
        {
            _isTransitionStart = false;
        }
    }

    public void StartCooldown()
    {
        _fill = 0f;
        _isTransitionStart = true;

    }
}