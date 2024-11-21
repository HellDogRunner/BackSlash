using System;
using UnityEngine;
using static PlayerStates;

public class PlayerStateMachine : MonoBehaviour
{
	private bool _willInteract;

	[HideInInspector] public EState CurrentState;
	
	public event Action<EState> OnChangeState;
	public event Action OnPause;
	public event Action OnExplore;
	public event Action OnInteract;
	
	private void ChangeState(EState state)
	{
		CurrentState = state;
		OnChangeState?.Invoke(state);
	}
	
	public void Start()
	{
		ChangeState(EState.Start);
	}
	
	public void Pause()
	{
		if (CurrentState == EState.Interact) _willInteract = true;
		ChangeState(EState.Pause);
		OnPause?.Invoke();
	}
	
	public void Walk()
	{
		ChangeState(EState.Walk);
	}	
	
	public void Explore()
	{
		if (_willInteract)
		{
			_willInteract = false;
			Interact();
			return;
		}
		
		ChangeState(EState.Explore);
		OnExplore?.Invoke();
	}	
	
	public void Combat()
	{
		ChangeState(EState.Combat);
	}	
	
	public void Interact()
	{
		if (CurrentState != EState.Combat || CurrentState != EState.Loot)
		{
			ChangeState(EState.Interact);
			OnInteract?.Invoke();
		}
	}
	
	public void Loot()
	{
		ChangeState(EState.Loot);
	}	
}
