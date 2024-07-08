using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class StartBasicWindow : BasicWindow
    {
        protected CanvasGroup _canvasGroup;

        protected SceneTransitionManager _sceneTransition;
        protected StartAnimationController _animationController;
        protected StartWindowsManager _windowsManager;

        [Inject]
        protected virtual void Construct(SceneTransitionManager sceneTransition, StartAnimationController animationController, StartWindowsManager windowsManager)
        {
            _sceneTransition = sceneTransition;
            _animationController = animationController;
            _windowsManager = windowsManager;

            _canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}