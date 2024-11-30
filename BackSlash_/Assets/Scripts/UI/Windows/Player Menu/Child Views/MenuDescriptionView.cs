using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
	public class MenuDescriptionView : MonoBehaviour
	{
		[SerializeField] private CanvasGroup cg; 
		[Space]
		[SerializeField] private TMP_Text TMP;
		
		[Header("Settings")]
		[SerializeField] private float duration;
		[SerializeField] private float delay;
		[Space]
		[SerializeField] private Vector3 startScale = new Vector3(0.9f, 0.9f, 0.9f);
		
		private Sequence show;

		private void Awake()
		{
			Hide();
		}
		
		public void Show(string text, Vector3 itemPos)
		{
			TMP.text = text;
			transform.position = itemPos;
			
			show = DOTween.Sequence();
			show.AppendCallback(() => 
			{
				cg.DOFade(1, duration).SetUpdate(true);
				cg.transform.DOScale(1, duration).SetUpdate(true);
			}).SetUpdate(true).SetDelay(delay).SetEase(Ease.InSine);
		}
		
		public void Hide()
		{
			KillTween(show);
			cg.alpha = 0;
			cg.transform.localScale = startScale;
		}
		
		private void KillTween(Tween tween)
		{
			if (tween.IsActive()) tween.Kill();
		}
		
		private void OnDestroy()
		{
			KillTween(show);
		}
	}
}
