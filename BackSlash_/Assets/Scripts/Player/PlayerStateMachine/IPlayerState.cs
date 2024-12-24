using System;

public interface IPlayerState
{
	bool CanEnter();
	void Enter();
	void Exit();
}