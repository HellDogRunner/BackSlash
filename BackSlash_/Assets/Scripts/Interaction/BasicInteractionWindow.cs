using RedMoonGames.Basics;
using RedMoonGames.Window;
using Scripts.Player;
using Zenject;

public class BasicInteractionWindow : BasicWindow
{
	protected InteractionSystem _interactionSystem;
	protected TimeController _time;

	[Inject]
	private void Construct(TimeController time, InteractionSystem interactionSystem)
	{
		_interactionSystem = interactionSystem;
		_time = time;
	}

	protected override void OnEnable()
	{		
		base.OnEnable();
		_interactionSystem.TryRemoveWindow(this);
		
		_uiInputs.OnEscapeKeyPressed -= Hide;
		_time.OnPause += Pause;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_time.OnPause -= Pause;
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
	
	protected void Pause(bool pause)
	{
		if (pause) _interactionSystem.DisableWindow(pause, gameObject);
	}
}
