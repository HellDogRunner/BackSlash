using RedMoonGames.Window;
using Scripts.Player;
using UnityEngine;
using Zenject;

public class BasicMenuController : MonoBehaviour
{
	protected SceneTransition _sceneTransition;
	protected WindowAnimator _animator;
	protected WindowService _windowService;
	protected UIActionsController _pauseInputs;

	[Inject]
	protected void Construct(SceneTransition sceneTransition, WindowAnimator windowAnimation, WindowService windowService, UIActionsController actionsController)
	{
		_windowService = windowService;
		_animator = windowAnimation;
		_pauseInputs = actionsController;
		_sceneTransition = sceneTransition;
	}

	protected void CloseWindow(WindowHandler window)
	{
		_windowService.ReturnIWindow(window).Close();
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
}