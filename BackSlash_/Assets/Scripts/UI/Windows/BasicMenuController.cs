using RedMoonGames.Window;
using Scripts.Player;
using UnityEngine;
using Zenject;

public class BasicMenuController : MonoBehaviour
{
    protected SceneTransitionService _sceneTransition;
    protected WindowAnimationService _windowAnimation;
    protected WindowService _windowService;
    protected UIActionsController _pauseInputs;

    [Inject]
    private void Construct(WindowAnimationService windowAnimation, SceneTransitionService sceneTransition, WindowService windowService, UIActionsController actionsController)
    {
        _windowService = windowService;
        _windowAnimation = windowAnimation;
        _sceneTransition = sceneTransition;
        _pauseInputs = actionsController;
    }

    protected void CloseWindow()
    {
        _windowService.ReturnActiveWindow()?.Close();
    }

    protected void SceneTransitionHide()
    {
        _sceneTransition.gameObject.SetActive(false);
    }

    public void ChangeScene(string sceneName)
    {
        PrepareToChangeScene();
        _sceneTransition.gameObject.SetActive(true);
        _sceneTransition.SwichToScene(sceneName);
    }

    protected void PrepareToChangeScene()
    {
        _pauseInputs.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 0;
    }

    protected void SwitchVisible(bool visible)
    {
        Cursor.visible = visible;
    }
}