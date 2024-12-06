using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Menu
{
	public class BasicMenuChildView : MenuElement, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] protected CanvasGroup frame;
		
		protected MenuView view;
		
		protected float offsetY;
		protected string descriptionText;
		
		protected Tween t_frame;
		
		public virtual void SetView(MenuView view)
		{
			this.view = view;
		}
		
		protected virtual void Awake()
		{
			frame.alpha = 0;
			
			var rt = transform as RectTransform;
			offsetY = rt.rect.height / 2;
		}
		
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			view.Description.Show(descriptionText, transform.position, offsetY);
			view.KillTween(t_frame);
			t_frame = frame.DOFade(1, view.Duration).SetUpdate(true).SetDelay(view.Delay);
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			view.Description.Hide();
			view.KillTween(t_frame);
			frame.alpha = 0;
		}
		
		protected virtual void OnDestroy()
		{
			view.KillTween(t_frame);
		}
	}
}