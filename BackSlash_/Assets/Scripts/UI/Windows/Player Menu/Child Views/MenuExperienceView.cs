using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Menu
{
	public class MenuExperienceView : BasicMenuChildView
	{
		[SerializeField] private CanvasGroup value;
		[SerializeField] private Image fill;
		
		[Header("TMPs")]
		[SerializeField] private TMP_Text comboPoints;
		[SerializeField] private TMP_Text currentExpTMP;
		[SerializeField] private TMP_Text maxExpTMP;
		
		[Header("Settings")]
		[SerializeField] private float duration;
		[SerializeField] private float delay;
		
		// Create exp system //
		private int currentExp = 450;	// fill animation on awake ??
		private int maxExp = 1000;
		
		private Tween t_fill;
		private Tween t_value;

		protected override void Awake()
		{
			base.Awake();
			
			value.alpha = 0;
			fill.fillAmount = 0;
			
			// get exp information //
			// start
			comboPoints.text = 3.ToString();
			
			var fraction = currentExp / (float)maxExp;
			// end //
			
			currentExpTMP.text = currentExp.ToString();
			maxExpTMP.text = maxExp.ToString();

			descriptionText = "Experience\nN avilable points";
			
			Fill(fraction);
		}
		
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			view.KillTween(t_value);
			t_value = value.DOFade(1, 0).SetUpdate(true).SetDelay(view.Delay);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			view.KillTween(t_value);
			value.alpha = 0;
		}
		
		private void Fill(float fraction)
		{
			t_fill = fill.DOFillAmount(fraction, duration).
				SetUpdate(true).SetEase(Ease.OutCubic).SetDelay(delay);
		}
		
		protected override void OnDestroy()
		{
			base.OnDestroy();
			view.KillTween(t_fill);
		}
	}
}
