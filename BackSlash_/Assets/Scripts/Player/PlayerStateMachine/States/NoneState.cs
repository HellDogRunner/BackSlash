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
		}
		public void Update() {}
		
		public void Exit() {}
		
		public bool CanEnterInAir()
		{
			return true;
		}

		public bool CanJump()
		{
			return true;
		}
		
		public bool CanMove()
		{
			return true;
		}
	}
}
