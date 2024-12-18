using System;
using UnityEngine;
using static PlayerStates;

public class PlayerStateMachine : MonoBehaviour
{
	private bool _willInteract;

	[HideInInspector] public EState State;
	
	public event Action<EState> OnChangeState;
	public event Action OnPause;
	public event Action OnExplore;
	public event Action OnInteract;
	
	private void ChangeState(EState state)
	{
		State = state;
		OnChangeState?.Invoke(state);
	}
	
	public void Start()
	{
		ChangeState(EState.Start);
	}
	
	public void Pause()
	{
		if (State == EState.Interact) _willInteract = true;
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
		if (State != EState.Combat || State != EState.Loot)
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
