namespace Scripts.Player
{
	public class LootState : IPlayerState
	{
		protected PlayerStateController _player;
		
		public LootState(PlayerStateController player)
		{
			_player = player;
		}

		public bool CanEnter()
		{
			return !_player.Movement.Air && _player.TargetLock.Target == null;
		}

		public void Enter()
		{
			_player.State = EPlayerState.Loot;
			_player.SendLoot(true);
		}

		public void Exit()
		{
			_player.SendLoot(false);
		}
	}
}
