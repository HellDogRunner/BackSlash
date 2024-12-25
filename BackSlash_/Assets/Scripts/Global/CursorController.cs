using Scripts.Player;
using UnityEngine;
using Zenject;

public class CursorController : MonoBehaviour
{
	private PlayerStateController _stateController;
	private UiInputsController _uiInputs;
	private TimeController _time;

	[Inject] private void Construct(TimeController time, UiInputsController uiInputs, PlayerStateController stateController) 
	{
		_time = time;
		_uiInputs = uiInputs;
		_stateController = stateController;
	}

	private void OnEnable()
	{
		_stateController.OnInteract += Interact;
		_time.OnPause += Pause;
		_uiInputs.ShowCursor += Visible;
	}
	
	private void OnDisable()
	{
		_stateController.OnInteract -= Interact;
		_time.OnPause -= Pause;
		_uiInputs.ShowCursor -= Visible;
	}

	public void Pause(bool value)
	{
		if (value) Confine();
		else if (_stateController.State != EPlayerState.Interact) Lock();
	}
	
	private void Interact(bool value)
	{
		if (value) Confine();
		else Lock();
	}

	private void Lock()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	private void Confine()
	{
		Cursor.lockState = CursorLockMode.Confined;
	}
	
	private void Visible(bool show)
	{
		Cursor.visible = show;
	}
}
