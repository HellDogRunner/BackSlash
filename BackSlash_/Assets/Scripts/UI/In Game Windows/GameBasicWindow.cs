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

            _windowManager.OnWindowClosed += HideCurrentWindow;
            _windowManager.OnPausing += EnablePause;
        }

        protected virtual void GameWindowDestroy()
        {

        }

        protected virtual void HideCurrentWindow(WindowHandler handler)
        {
            _animationController.HideWindowAnimation(_canvasGroup, handler);
        }

        protected virtual void EnablePause()
        {

        }

        protected virtual void OnDestroy()
        {
            GameWindowDestroy();
            _windowManager.OnWindowClosed -= HideCurrentWindow;
            _windowManager.OnPausing -= EnablePause;
        }
    }
}