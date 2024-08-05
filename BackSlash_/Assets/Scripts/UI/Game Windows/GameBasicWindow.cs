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
        protected CanvasGroup _canvasGroup;

        protected GameWindowsController _windowManager;
        protected WindowAnimationService _animationController;
        protected SceneTransitionController _sceneTransition;
        protected AudioManager _audioManager;
        protected UIController _controller;

        [Inject]
        protected virtual void Construct(GameWindowsController windowController, WindowAnimationService animationController, SceneTransitionController sceneTransition, AudioManager audioManager, UIController controller)
        {
            _animationController = animationController;
            _sceneTransition = sceneTransition;
            _audioManager = audioManager;

            _windowManager = windowController;
            _windowManager.OnUnpausing += DisablePause;

            _controller = controller;

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected void SwitchWindows(WindowHandler close, WindowHandler open)
        {
            PlayClickSound();

            _windowManager.CloseWindow(close);
            _windowManager.OpenWindow(open);
        }

        protected void DisablePause(WindowHandler handler)
        {
            PlayClickSound();
            _animationController.HideWindowAnimation(_canvasGroup, handler);
        }

        protected void PlayClickSound()
        {
            _audioManager.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);
        }

        protected void PlayHoverSound()
        {
            _audioManager.PlayGenericEvent(FMODEvents.instance.UIHoverEvent);
        }

        protected void ChangeSliderValue(Slider slider, TMP_Text value, int multiplier)
        {
            PlayHoverSound();
            value.text = (slider.value * multiplier).ToString();
        }
    }
}