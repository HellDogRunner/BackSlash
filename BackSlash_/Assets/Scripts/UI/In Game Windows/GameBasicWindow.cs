using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
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

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Awake()
        {
            _windowManager.OnUnpause += HideCurrentWindow;
            _windowManager.OnPausing += EnablePause;
        }

        protected virtual void HideCurrentWindow()
        {

        }

        protected virtual void EnablePause()
        {

        }

        private void OnDestroy()
        {
            _windowManager.OnUnpause -= HideCurrentWindow;
            _windowManager.OnPausing -= EnablePause;
        }
    }
}