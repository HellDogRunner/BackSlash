public interface IPlayerState
{
	void Enter();
	void Update();
	void Exit();
	void SetInterruptible();
	void SetInactive();
	bool CanBeInterrupt();
	bool CanEnterInAir();
}