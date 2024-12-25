namespace Scripts.Player
{
	public class BlockState : IPlayerState
	{
		private PlayerStateController _player;
		private EPlayerState state = EPlayerState.Block;
		
		public BlockState(PlayerStateController player){ _player = player; }
		public EPlayerState GetState() { return state; }

		public bool CanEnter()
		{
			return _player.State == EPlayerState.None;
		}

		public void Enter()
		{
			_player.State = state;
			_player.SendBlock(true);
		}

		public void Exit()
		{
			_player.SendBlock(false);
		}
	}
}
