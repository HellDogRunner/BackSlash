using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Menu
{
	public class BasicMenuChildView : MenuElement, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] protected CanvasGroup frame;
		
		protected MenuView view;
		
		protected string descriptionText;
		protected Vector2 DescriptionPivot;
		
		protected Tween t_frame;
		
		public virtual void AwakeSet(MenuView view)
		{
			this.view = view;
			DescriptionPivot = view.SetPivot(transform as RectTransform);
		}
		
		protected virtual void Awake()
		{
			frame.alpha = 0;
		}
		
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			view.Description.SetPosition(transform.position, DescriptionPivot);
			view.Description.Show(descriptionText);
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