using RedMoonGames.Basics;
using RedMoonGames.Window;
using Zenject;

public class BasicInteractionWindow : BasicWindow
{
	protected InteractionSystem _interactionSystem;

	[Inject]
	private void Construct( InteractionSystem interactionSystem)
	{
		_interactionSystem = interactionSystem;
		_interactionSystem.SetActiveWindow(gameObject);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		
		_uiInputs.OnEscapeKeyPressed -= Hide;
	}

	protected void OpenWindow(WindowHandler handler)// TODO Needed????
	{
		var window = _windowService.GetWindowByHandler(handler) as CachedBehaviour;

		if (window == null) _windowService.TryOpenWindow(handler);
		else window.gameObject.SetActive(true);

		//_interactionSystem.TryAddWindow(this);
		gameObject.SetActive(false);
	}
	
	public void StopInteract()
	{
		_interactionSystem.StopInteract();
		Hide();
	}
}
