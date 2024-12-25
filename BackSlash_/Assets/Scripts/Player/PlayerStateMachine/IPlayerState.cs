using System;

public interface IPlayerState
{
	EPlayerState GetState();
	bool CanEnter();
	void Enter();
	void Exit();
}