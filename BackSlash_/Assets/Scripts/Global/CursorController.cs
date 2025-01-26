using Scripts.Player;
using UnityEngine;
using Zenject;

public class CursorController : MonoBehaviour
{
	private UiInputsController _uiInputs;
	private TimeController _time;

	[Inject] private void Construct(TimeController time, UiInputsController uiInputs) 
	{
		_time = time;
		_uiInputs = uiInputs;
	}

	private void OnEnable()
	{
		_time.OnPause += Pause;
		_uiInputs.ShowCursor += Visible;
	}
	
	private void OnDisable()
	{
		_time.OnPause -= Pause;
		_uiInputs.ShowCursor -= Visible;
	}

	public void Pause(bool value)
	{
		if (value) Confine();
		else Lock();
	}
	
	public void Interact(bool value)
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
