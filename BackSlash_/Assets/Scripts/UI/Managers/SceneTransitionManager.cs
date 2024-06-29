using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _loadingImage;
    [SerializeField] private TMP_Text _loadingText;

    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _loadingImageDuration = 1f;

    private static bool shouoldPlayOpeningAnimation = false;

    private AsyncOperation _loadingSceneOperation;

    private Sequence _sequence;

    private bool _isClockwise;

    private void Start()
    {
        _isClockwise = true;

        if (shouoldPlayOpeningAnimation)
        {
            PlayOpeningAnimation();
        }
    }

    public void SwichToScene(string sceneName)
    {
        PlayClosingAnimation();
        _loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _loadingSceneOperation.allowSceneActivation = false;
    }

    private void PlayOpeningAnimation()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _canvasGroup.alpha = 1f;
        _loadingText.alpha = 1f;
        shouoldPlayOpeningAnimation = false;

        _sequence = DOTween.Sequence();
        _sequence.AppendCallback(() =>
        {
            _loadingText.DOFade(0f, _fadeDuration);
            _loadingImage.DOFade(0f, _fadeDuration);
        });
        _sequence.AppendInterval(_fadeDuration);
        _sequence.Append(_canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.InExpo));
    }

    private void PlayClosingAnimation()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _canvasGroup.alpha = 0f;
        _loadingText.alpha = 0f;

        _sequence = DOTween.Sequence();
        _sequence.Append(_canvasGroup.DOFade(1f, _fadeDuration)).SetUpdate(true);
        _sequence.AppendCallback(() =>
        {
            _loadingText.DOFade(1f, _fadeDuration).SetUpdate(true);
            _loadingImage.DOFade(1f, _fadeDuration).SetUpdate(true).OnComplete(LoadingAnimation);
        });
    }

    private void LoadingAnimation()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(_loadingImage.DOFillAmount(1f, _loadingImageDuration)).SetUpdate(true);
        _sequence.AppendCallback(LoadingImageRotate).SetUpdate(true);
        _sequence.Append(_loadingImage.DOFillAmount(0f, _loadingImageDuration)).SetUpdate(true);
        _sequence.AppendCallback(LoadingImageRotate).SetUpdate(true);
        _sequence.AppendCallback(CheckSceneLoaded).SetUpdate(true);
        _sequence.SetLoops(-1);
    }

    private void LoadingImageRotate()
    {
        if (_isClockwise)
        {
            _isClockwise = false;
        }
        else
        {
            _isClockwise = true;
        }
        _loadingImage.fillClockwise = _isClockwise;
    }

    private void CheckSceneLoaded()
    {
        if (_loadingSceneOperation.progress == 0.9f)
        {
            _sequence.Kill();
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        shouoldPlayOpeningAnimation = true;
        _loadingSceneOperation.allowSceneActivation = true;
    }
}