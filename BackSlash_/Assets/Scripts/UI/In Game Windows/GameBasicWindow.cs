using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    [RequireComponent(typeof(CanvasGroup))]

    public class GameBasicWindow : BasicWindow
    {
        protected CanvasGroup _canvasGroup;

        protected GameWindowsManager _windowManager;
        protected UIAnimationController _animationController;
        protected SceneTransitionManager _sceneTransition;

        [Inject]
        protected virtual void Construct(GameWindowsManager windowController, UIAnimationController animationController, SceneTransitionManager sceneTransition)
        {
            _windowManager = windowController;
            _animationController = animationController;
            _sceneTransition = sceneTransition;

            _windowManager.OnUnpausing += DisablePause;
            _windowManager.OnPausing += EnablePause;
        }

        protected void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void DisablePause()
        {
        }

        protected virtual void EnablePause()
        {
        }

        protected virtual void OnDestroy()
        {
            _windowManager.OnUnpausing -= DisablePause;
            _windowManager.OnPausing -= EnablePause;
        }

    }
}