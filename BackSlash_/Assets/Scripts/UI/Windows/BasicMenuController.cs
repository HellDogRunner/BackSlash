using RedMoonGames.Window;
using Scripts.Player;
using UnityEngine;
using Zenject;

public class BasicMenuController : MonoBehaviour
{
	protected SceneTransition _sceneTransition;
	protected WindowAnimator _animator;
	protected WindowService _windowService;
	protected UiInputsController _uiInputs;

	[Inject]
	protected void Construct(SceneTransition sceneTransition, WindowAnimator windowAnimation, WindowService windowService, UiInputsController uiInputs)
	{
		_windowService = windowService;
		_animator = windowAnimation;
		_uiInputs = uiInputs;
		_sceneTransition = sceneTransition;
	}
	
	protected void SceneTransitionHide()
	{
		_sceneTransition.gameObject.SetActive(false);
	}

	public void ChangeScene(string sceneName)
	{
		_uiInputs.enabled = false;
		_sceneTransition.gameObject.SetActive(true);
		_sceneTransition.SwichToScene(sceneName);
	}
}
