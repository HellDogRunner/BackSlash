namespace Scripts.Player
{
	public class PauseState : IPlayerState
	{
		protected PlayerStateController _player;
		
		public PauseState(PlayerStateController player)
		{
			_player = player;
		}

		public bool CanEnter()
		{
			return true;
		}

		public void Enter()
		{
			_player.State = EPlayerState.Pause;
			_player.SendPause(true);
		}

		public void Exit()
		{
			_player.SendPause(false);
		}
	}
}
