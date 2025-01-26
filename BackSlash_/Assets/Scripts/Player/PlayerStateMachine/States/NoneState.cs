namespace Scripts.Player
{
	public class NoneState : BasicState, IPlayerState
	{
		public NoneState(PlayerStateController player)
		{
			_player = player;
		}

		public void Enter()
		{
			_interruptible = true;
			_player.State = EPlayerState.None;
			_player.Camera.SetCanRotate(true);
			_player.Movement.SetCanFall(true);
			_player.Movement.SetCanJump(true);
			_player.Movement.SetCanSprint(true);
		}
		public void Update() {}
		
		public void Exit() {}
		
		public bool CanEnterInAir()
		{
			return true;
		}
	}
}
