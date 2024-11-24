using RedMoonGames.Basics;
using RedMoonGames.Window;
using UnityEngine;
using Zenject;

public class BasicInteractionWindow : BasicWindow
{
	protected InteractionSystem _interactionSystem;
	protected PlayerStateMachine _playerSM;

	[Inject]
	private void Construct(PlayerStateMachine playerState, InteractionSystem interactionSystem)
	{
		_interactionSystem = interactionSystem;
		_playerSM = playerState;
	}

	protected override void OnEnable()
	{		
		base.OnEnable();
		_interactionSystem.TryRemoveWindow(this);
		
		_uiInputs.OnEscapeKeyPressed -= Hide;
		_playerSM.OnPause += Pause;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_playerSM.OnPause -= Pause;
	}

	protected void OpenWindow(WindowHandler handler)
	{
		var window = _windowService.GetWindowByHandler(handler) as CachedBehaviour;

		if (window == null) _windowService.TryOpenWindow(handler);
		else window.gameObject.SetActive(true);


		_interactionSystem.TryAddWindow(this);
		gameObject.SetActive(false);
	}
	
	public void HideWindow()
	{
		_interactionSystem.StopInteract();
		Hide();
	}
	
	protected void Pause()
	{
		_interactionSystem.DisableWindow(gameObject);
	}
}
