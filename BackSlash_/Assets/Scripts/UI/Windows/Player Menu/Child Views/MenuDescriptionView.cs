using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Scripts.Menu
{
	public class MenuDescriptionView : MenuElement
	{
		[SerializeField] private CanvasGroup cg;
		[SerializeField] private Transform mask;
		[Space]
		[SerializeField] private TMP_Text TMP;
		
		[Header("Settings")]
		[SerializeField] private Vector3 startScale = new Vector3(0.9f, 0.9f, 0.9f);
		
		private float duration;
		private float delay;
		
		private MenuView view;
		
		private RectTransform rect;

		
		private Tween fade;
		private Tween scale;

		public void OnAwake(MenuView view)
		{
			this.view = view;
			duration = view.Duration;
			delay = view.Delay;
			rect = gameObject.transform as RectTransform;
			
			Hide();
		}
		
		public void Show(string text)
		{
			TMP.text = text;

			fade = cg.DOFade(1, duration).SetUpdate(true).SetDelay(delay).SetEase(Ease.InSine);
			scale = mask.DOScale(1, duration).SetUpdate(true).SetDelay(delay).SetEase(Ease.InSine);
		}
		
		public void Hide()
		{
			view.KillTween(fade);
			view.KillTween(scale);
			cg.alpha = 0;
			mask.localScale = startScale;
		}
		
		public void SetPosition(Vector3 itemPos, Vector2 pivot)
		{
			rect.pivot = pivot;
			transform.position = itemPos;
		}
		
		private void OnDestroy()
		{
			view.KillTween(fade);
			view.KillTween(scale);
		}
	}
}
