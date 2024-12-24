namespace Scripts.Player
{
	public class NoneState : IPlayerState
	{
		protected PlayerStateController _player;
		
		public NoneState(PlayerStateController player)
		{
			_player = player;
		}

		public bool CanEnter()
		{
			return true;
		}

		public void Enter()
		{
			_player.State = EPlayerState.None;
			_player.SendNone(true);
		}

		public void Exit()
		{
			_player.SendNone(false);
		}
		
	}
}
