using Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class BasicWindow : IBasicWindow
	{
		[SerializeField] protected bool _isMainMenu;
		[SerializeField] protected Button _close;
		
		protected CanvasGroup _canvasGroup;
		protected WindowHandler _thisHandler;

		protected WindowService _windowService;
		protected WindowAnimator _animator;
		protected AudioController _audioController;
		protected UiInputsController _uiInputs;

		[Inject]
		private void Construct(WindowService windowService, WindowAnimator animator, AudioController audioController, UiInputsController uiInputs)
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			_audioController = audioController;
			_windowService = windowService;
			_animator = animator;
			_uiInputs = uiInputs;
		}
		
		protected virtual void OnEnable()
		{
			_uiInputs.OnEscapeKeyPressed += Hide;
			_animator.OnShowed += Showed;
			_animator.OnHided += Hided;
			
			if (_close) _close.onClick.AddListener(Hide);	
		}
		
		protected virtual void OnDisable()
		{
			_uiInputs.OnEscapeKeyPressed -= Hide;
			_animator.OnShowed -= Showed;
			_animator.OnHided -= Hided;
			
			if (_close) _close.onClick.RemoveListener(Hide);	
		}
		
		public override void SetHandler(WindowHandler handler)
		{
			_thisHandler = handler;
		}
		
		public override void Show(bool delay = false) 
		{
			if (!_animator.Active())
			{
				_animator.ShowWindow(_thisHandler, _canvasGroup, delay);
				PlayClickSound();
			}
		}
		
		protected virtual void Showed(WindowHandler handler)
		{
		}
		
		protected virtual void Hide()
		{
			if (_isMainMenu)
			{
				Close();
			}
			else if (!_animator.Active())
			{
				_animator.HideWindow(_thisHandler, _canvasGroup);	
				PlayClickSound();
			}
		}
		
		protected virtual void Hided(WindowHandler handler)
		{
			if (handler == _thisHandler)
			{
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
			_audioController.PlayGenericEvent(FMODEvents.instance.UIButtonClick);
		}

		protected void PlayHoverSound()
		{
			_audioController.PlayGenericEvent(FMODEvents.instance.UIButtonHover);
		}

		protected void ChangeSliderValue(Slider slider, TMP_Text value, int multiplier)
		{
			PlayHoverSound();
			value.text = (slider.value * multiplier).ToString();
		}
	}
}
