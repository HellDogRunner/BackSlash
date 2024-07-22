using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class StartBasicWindow : BasicWindow
    {
        protected CanvasGroup _canvasGroup;

        protected SceneTransitionController _sceneTransition;
        protected StartAnimationController _animationController;
        protected StartWindowsController _windowsManager;

        [Inject]
        protected virtual void Construct(SceneTransitionController sceneTransition, StartAnimationController animationController, StartWindowsController windowsManager)
        {
            _sceneTransition = sceneTransition;
            _animationController = animationController;
            _windowsManager = windowsManager;

            _canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}