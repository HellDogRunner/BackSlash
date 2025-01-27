using RedMoonGames.Window;
using Scripts.Player;
using UnityEngine;
using Zenject;

public class BasicMenuController : MonoBehaviour
{
	protected WindowService _windowService;
	protected UiInputsController _uiInputs;
	protected WindowAnimator _animator; 

	[Inject]
	protected void Construct(WindowAnimator animator, WindowService windowService, UiInputsController uiInputs)
	{
		_windowService = windowService;
		_animator = animator;
		_uiInputs = uiInputs;
	}
}
