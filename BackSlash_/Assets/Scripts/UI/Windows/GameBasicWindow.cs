using Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	[RequireComponent(typeof(CanvasGroup))]
	public class GameBasicWindow : BasicWindow
	{
		[SerializeField] protected Button _close;
		
		protected CanvasGroup _canvasGroup;
		protected bool _callHide;

		protected WindowService _windowService;
		protected WindowAnimator _animator;
		protected AudioController _audioController;
		protected UIActionsController _uiInputs;

		[Inject]
		private void Construct(WindowService windowService, WindowAnimator animator, AudioController audioController, UIActionsController uiInputs)
		{
			_animator = animator;
			_audioController = audioController;
			_uiInputs = uiInputs;
			_windowService = windowService;
		}
		
		protected virtual void Awake() 
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			
			_windowService.OnShowWindow += Show;
			_animator.OnShowed += ShowHandler;
			_animator.OnHided += TryClose;
			_uiInputs.OnEscapeKeyPressed += Hide;
			
			if (_close) _close.onClick.AddListener(Hide);	
		}
		
		protected virtual void OnDestroy()
		{
			_windowService.OnShowWindow -= Show;
			_animator.OnShowed -= ShowHandler;
			_animator.OnHided -= TryClose;
			_uiInputs.OnEscapeKeyPressed -= Hide;
			
			if (_close) _close.onClick.RemoveListener(Hide);	
		}
		
		protected void Show(bool pause) 
		{
			if (!_animator.Active())
			{
				PlayClickSound();	// Need?
				_animator.ShowWindow(_canvasGroup);
				if (pause) _windowService.Pause();
			}
		}
		
		protected virtual void Hide()
		{
			if (!_animator.Active())
			{
				_callHide = true;
				PlayClickSound();	// Need?
				_animator.HideWindow(_canvasGroup);
				_windowService.Unpause();
			}
		}
		
		protected virtual void ShowHandler() {}
		
		protected virtual void TryClose()
		{
			if (_callHide)
			{
				_callHide = false;
				Close();
			}
		}
		
		public void ReplaceWindow(IWindow window, WindowHandler handler)
		{
			PlayClickSound();
			
			window.Close();
			_windowService.TryOpenWindow(handler);
		}
		
		protected void SwitchView()
		{
			_canvasGroup.alpha = _canvasGroup.alpha == 0 ? 1 : 0;
		}

		protected void PlayClickSound()
		{
			_audioController.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);
		}

		protected void PlayHoverSound()
		{
			_audioController.PlayGenericEvent(FMODEvents.instance.UIHoverEvent);
		}

		protected void ChangeSliderValue(Slider slider, TMP_Text value, int multiplier)
		{
			PlayHoverSound();
			value.text = (slider.value * multiplier).ToString();
		}
	}
}
