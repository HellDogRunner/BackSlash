using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Scripts.Menu
{
	public class MenuDescriptionView : MonoBehaviour
	{
		[SerializeField] private CanvasGroup cg; 
		[Space]
		[SerializeField] private TMP_Text TMP;
		
		[Header("Settings")]
		[SerializeField] private float duration;
		
		private PlayerMenu menu;
		
		private Tween fade;
		private Tween follow;

		private void Awake()
		{
			cg.alpha = 0;
		}

		public void SetOnAwake(PlayerMenu menu)
		{
			this.menu = menu;
		}
		
		public void Show(string text)
		{
			TMP.text = text;
			transform.position = Input.mousePosition;
			
			KillTween(fade);
			fade = cg.DOFade(1, duration).SetUpdate(true);
		}
		
		public void Follow()
		{
			KillTween(follow);
			follow = transform.DOMove(Input.mousePosition, duration).SetUpdate(true);
		}
		
		public void Hide()
		{
			KillTween(fade);
			fade = cg.DOFade(0, duration).SetUpdate(true);
		}
		
		private void KillTween(Tween tween)
		{
			if (tween.IsActive()) tween.Kill();
		}
		
		private void OnDestroy()
		{
			KillTween(fade);
			KillTween(follow);
		}
	}
}
