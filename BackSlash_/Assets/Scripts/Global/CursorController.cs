using Scripts.Player;
using UnityEngine;
using Zenject;
using static PlayerStates;

public class CursorController : MonoBehaviour
{
	private PlayerStateMachine _playerState;
	private UiInputsController _uiInputs;

	[Inject] private void Construct(UiInputsController uiInputs, PlayerStateMachine playerState) 
	{
		_uiInputs = uiInputs;
		_playerState = playerState;
	}

	private void OnEnable()
	{
		_playerState.OnChangeState += SwitchCursor;
		_uiInputs.ShowCursor += Visible;
	}
	
	private void OnDisable()
	{
		_playerState.OnChangeState -= SwitchCursor;
		_uiInputs.ShowCursor -= Visible;
	}

	private void SwitchCursor(EState state)
	{
		if (state == EState.Pause || state == EState.Interact) Confine();
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
