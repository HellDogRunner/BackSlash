using RedMoonGames.Window;
using Scripts.Player;
using UnityEngine;
using Zenject;

public class BasicMenuController : MonoBehaviour
{
	protected SceneTransition _sceneTransition;
	protected WindowService _windowService;
	protected UiInputsController _uiInputs;
	protected WindowAnimator _animator; 

	[Inject]
	protected void Construct(SceneTransition sceneTransition, WindowAnimator animator, WindowService windowService, UiInputsController uiInputs)
	{
		_sceneTransition = sceneTransition;
		_windowService = windowService;
		_animator = animator;
		_uiInputs = uiInputs;
		
		_sceneTransition.gameObject.SetActive(true);
	}
	
	protected virtual void OnEnable()
	{
		_sceneTransition.OnWindowHide += HideSceneTransition;
	}
	
	protected virtual void OnDisable()
	{
		_sceneTransition.OnWindowHide -= HideSceneTransition;
	}
	
	protected void HideSceneTransition()
	{
		_sceneTransition.gameObject.SetActive(false);
	}
}
