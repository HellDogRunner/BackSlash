using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDAnimationService : MonoBehaviour
{
	[SerializeField] private TMP_Text _currency;
	[SerializeField] private Image _targetIcon;
	
	[Header("Canvas Groups")]
	[SerializeField] private CanvasGroup _hudCG;
	[SerializeField] private CanvasGroup _overlayCG;

	[Header("Animation Settings")]
	[SerializeField] private float _showDelay = 1f;
	[SerializeField] private float _fadeDuration = 0.5f;

	private Transform _target;
	private Camera _camera;

	private Tween _overlay;
	private Tween _hud;

	private void Awake()
	{
		_camera = Camera.main;
		
		_hudCG.alpha = 1;
		_overlayCG.alpha = 0;
	}
	
	public void ShowOverlay()
	{
		ShowAnimation(_overlayCG, _overlay);
	}

	public void HideOverlay()
	{
		HideAnimation(_overlayCG, _overlay);
	}
	
	public void ShowHUD()
	{
		// TryKillTween(_hud);
		
		// _hud = _hudCG.DOFade(1, _fadeDuration);
		// _hudCG.alpha = 1;
		ShowAnimation(_hudCG, _hud, false);
	}

	public void HideHUD()
	{
		_hudCG.alpha = 0;
	}

	private void ShowAnimation(CanvasGroup cg, Tween tween, bool doDelay = true)
	{
		if (!cg.gameObject.activeSelf) cg.gameObject.SetActive(true);
		var delay = doDelay ? _showDelay : 0;
		TryKillTween(tween);
		
		tween = cg.DOFade(1, _fadeDuration).SetEase(Ease.InQuart).SetDelay(delay);
	}

	private void HideAnimation(CanvasGroup cg, Tween tween)
	{
		TryKillTween(tween);
		
		tween = cg.DOFade(0, _fadeDuration).SetEase(Ease.InQuart).
			OnComplete(() => cg.gameObject.SetActive(false));
	}
	
	public void SetTarget(Transform target)
	{
		_target = target;
	}

	public void Targeting() 
	{
		_targetIcon.transform.position = _camera.WorldToScreenPoint(_target.position);
	}
	
	public void ShowTargetIcon(bool show)
	{
		_targetIcon.gameObject.SetActive(show);
	}

	public void SetCurrency(int value)
	{
		_currency.text = value.ToString();
	}

	public TMP_Text GetCurrency()
	{
		return _currency;
	}
	
	private void TryKillTween(Tween tween)
	{
		if (tween.IsActive()) tween.Kill();
	}	
}
