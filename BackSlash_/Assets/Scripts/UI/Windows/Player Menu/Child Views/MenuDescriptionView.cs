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
		[SerializeField] private float distance = 10;
		[SerializeField] private Vector3 startScale = new Vector3(0.9f, 0.9f, 0.9f);
		
		private float duration;
		private float delay;
		
		private MenuView view;
		
		private RectTransform rect;
		private Vector2 resolution;
		
		private Tween fade;
		private Tween scale;

		public void OnAwake(MenuView view)
		{
			this.view = view;
			duration = view.Duration;
			delay = view.Delay;
			rect = gameObject.transform as RectTransform;
			resolution = new Vector2(Screen.width, Screen.height);
			
			Hide();
		}
		
		public void Show(string text, Vector3 itemPos, float offsetY)
		{
			TMP.text = text;
			
			SetPosition(itemPos, offsetY);
			
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
		
		private void SetPosition(Vector3 itemPos, float offsetY)
		{
			if (itemPos.y < resolution.y / 2)
			{
				rect.pivot = new Vector2(0.5f, 0);
				itemPos.y += distance + offsetY;
			}
			else
			{
				rect.pivot = new Vector2(0.5f, 1);
				itemPos.y -= distance + offsetY;
			}
			transform.position = itemPos;
		}
		
		private void OnDestroy()
		{
			view.KillTween(fade);
			view.KillTween(scale);
		}
	}
}
