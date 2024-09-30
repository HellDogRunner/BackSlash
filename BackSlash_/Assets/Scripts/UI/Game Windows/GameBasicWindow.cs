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
        protected GameWindowsController _windowsController;
        protected WindowAnimationService _animationService;
        protected SceneTransitionService _sceneTransition;
        protected AudioController _audioController;
        protected UIPauseInputs _pauseInput;

        protected CanvasGroup _canvasGroup;

        [Inject]
        protected virtual void Construct(GameWindowsController windowController, WindowAnimationService animationController, SceneTransitionService sceneTransition, AudioController audioManager, UIPauseInputs pauseInput)
        {
            _animationService = animationController;
            _sceneTransition = sceneTransition;
            _audioController = audioManager;
            _pauseInput = pauseInput;

            _windowsController = windowController;
            _windowsController.OnUnpausing += DisablePause;
            _windowsController.OnPausing += EnablePause;

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected void SwitchWindows(WindowHandler close, WindowHandler open)
        {
            PlayClickSound();

            _windowsController.CloseWindow(close);
            _windowsController.OpenWindow(open);
        }

        protected void DisablePause(WindowHandler handler)
        {
            PlayClickSound();
            _animationService.HideWindowAnimation(_canvasGroup, handler);
        }

        protected virtual void EnablePause()
        {
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