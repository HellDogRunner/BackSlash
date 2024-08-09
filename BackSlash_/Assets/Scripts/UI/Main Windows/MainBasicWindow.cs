using Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
    public class MainBasicWindow : BasicWindow
    {
        protected MainWindowsController _windowsController;
        protected SceneTransitionService _sceneTransition;
        protected MainAnimationService _animationService;
        protected AudioController _audioController;
        protected UIController _uIController;

        protected CanvasGroup _canvasGroup;

        [Inject]
        protected virtual void Construct(MainWindowsController windowController, MainAnimationService animationService, SceneTransitionService sceneTransition, AudioController audioController, UIController uIController)
        {
            _animationService = animationService;
            _sceneTransition = sceneTransition;
            _audioController = audioController;
            _uIController = uIController;
            _windowsController = windowController;

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected void SwitchWindows(WindowHandler close, WindowHandler open)
        {
            PlayClickSound();

            _windowsController.CloseWindow(close);
            _windowsController.OpenWindow(open);
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