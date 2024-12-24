namespace Scripts.Player
{
	public class BlockState : IPlayerState
	{
		protected PlayerStateController _player;
		
		public BlockState(PlayerStateController player)
		{
			_player = player;
		}

		public bool CanEnter()
		{
			return _player.State == EPlayerState.None || _player.Movement.Air;
		}

		public void Enter()
		{
			_player.State = EPlayerState.Block;
			_player.SendBlock(true);
		}

		public void Exit()
		{
			_player.SendBlock(false);
		}
	}
}
