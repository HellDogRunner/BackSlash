using Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	[RequireComponent(typeof(CanvasGroup))]
	public class BasicWindow : IBasicWindow
	{
		[SerializeField] protected Button _close;
		
		protected CanvasGroup _canvasGroup;
		protected bool _callClose;

		protected WindowService _windowService;
		protected WindowAnimator _animator;
		protected AudioController _audioController;
		protected UiInputsController _uiInputs;

		[Inject]
		private void Construct(WindowService windowService, WindowAnimator animator, AudioController audioController, UiInputsController uiInputs)
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			
			_audioController = audioController;
			_animator = animator;
			_uiInputs = uiInputs;
			_windowService = windowService;
		}
		
		protected virtual void OnEnable()
		{
			_windowService.OnShowWindow += Show;
			_uiInputs.OnEscapeKeyPressed += Hide;
			_animator.OnShowed += Showed;
			_animator.OnHided += TryClose;
			
			if (_close) _close.onClick.AddListener(Hide);	
		}
		
		protected virtual void OnDisable()
		{
			_windowService.OnShowWindow -= Show;
			_uiInputs.OnEscapeKeyPressed -= Hide;
			_animator.OnShowed -= Showed;
			_animator.OnHided -= TryClose;
			
			if (_close) _close.onClick.RemoveListener(Hide);	
		}
		
		protected virtual void Show(bool pause, float delay) 
		{
			if (!_animator.Active())
			{
				PlayClickSound();	// Need?
				_animator.ShowWindow(_canvasGroup, delay);
				if (pause) _windowService.Pause();
			}
		}
		
		protected virtual void Hide()
		{
			if (!_animator.Active())
			{
				_callClose = true;
				PlayClickSound();	// Need?
				_animator.HideWindow(_canvasGroup);
				_windowService.Unpause();
			}
		}
		
		protected virtual void Showed() {}
		
		protected virtual void TryClose()
		{
			if (_callClose)
			{
				_callClose = false;
				Close();
			}
		}
		
		protected void ReplaceWindow(IWindow window, WindowHandler handler)
		{
			PlayClickSound();
			
			window.Close();
			_windowService.TryOpenWindow(handler);
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
