using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameBasicWindow : BasicWindow
    {
        protected CanvasGroup _canvasGroup;

        protected GameWindowsController _windowManager;
        protected UIAnimationController _animationController;
        protected SceneTransitionController _sceneTransition;
        protected AudioManager _audioManager;

        [Inject]
        protected virtual void Construct(GameWindowsController windowController, UIAnimationController animationController, SceneTransitionController sceneTransition, AudioManager audioManager)
        {
            _windowManager = windowController;
            _animationController = animationController;
            _sceneTransition = sceneTransition;
            _audioManager = audioManager;

            _windowManager.OnUnpausing += DisablePause;
            _windowManager.OnPausing += EnablePause; 

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void DisablePause()
        {
        }

        protected virtual void EnablePause()
        {
        }

        protected void PlayClickSound()
        {
            _audioManager.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);
        }

        protected virtual void OnDestroy()
        {
            _windowManager.OnUnpausing -= DisablePause;
            _windowManager.OnPausing -= EnablePause;
        }

    }
}