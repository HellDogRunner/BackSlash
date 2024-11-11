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
		[SerializeField] protected bool _IsMainMenu;

		protected CanvasGroup _canvasGroup;

		protected WindowService _windowService;
		protected WindowAnimationService _animationService;
		protected AudioController _audioController;
		protected UIActionsController _pauseInputs;

		[Inject]
		private void Construct(WindowService windowService, WindowAnimationService animationService, AudioController audioController, UIActionsController pauseInputs)
		{
			_canvasGroup = GetComponent<CanvasGroup>();

			_animationService = animationService;
			_audioController = audioController;
			_pauseInputs = pauseInputs;

			_windowService = windowService;
			_windowService.OnHideWindow += DisablePause;
		}

		protected void EnablePause() 
		{
			_animationService.ShowWindowAnimation(_canvasGroup);
		}

		protected void DisablePause(WindowHandler handler)
		{
			PlayClickSound();
			_animationService.HideWindowAnimation(_canvasGroup, handler);
		}

		public void OpenWindow(WindowHandler window)
		{
			PlayClickSound();
			
			_windowService.CloseActiveWindow();
			_windowService.TryOpenWindow(window);
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

		private void OnDestroy()
		{
			_windowService.OnHideWindow -= DisablePause;
		}
	}
}