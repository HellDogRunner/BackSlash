using Scripts.Entity;

public interface IPlayerState
{
	void Enter();
	void Update();
	void Exit();
	void HitTaken(AttackModel attack);
	void SetInterruptible();
	void SetInactive();
	bool CanBeInterrupt();
	bool CanEnterInAir();
}