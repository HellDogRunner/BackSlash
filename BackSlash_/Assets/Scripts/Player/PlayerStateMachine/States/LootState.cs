namespace Scripts.Player
{
	public class LootState : IPlayerState
	{
		private PlayerStateController _player;
		private EPlayerState state = EPlayerState.Loot;
		
		public LootState(PlayerStateController player){ _player = player; }
		public EPlayerState GetState() { return state; }
		
		public bool CanEnter()
		{
			return _player.State == EPlayerState.None;
		}

		public void Enter()
		{
			_player.State = state;
			_player.SendLoot(true);
		}

		public void Exit()
		{
			_player.SendLoot(false);
		}
	}
}
