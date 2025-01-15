namespace Scripts.Player
{
	public class LootState : BasicState, IPlayerState
	{
		public LootState(PlayerStateController player)
		{
			_player = player;
		}

		public void Enter()
		{
			_isActive = true;
			_player.State = EPlayerState.Loot;
			// send start loot state

		}

		public void Update()
		{
			AirCheck();
			if (!_isActive) _player.SetState(new NoneState(_player));
		}

		public void Exit()
		{
			// send end loot state
		}
		
		public bool CanEnterInAir()
		{
			return !_player.Movement.Air;
		}

		public bool CanJump()
		{
			return false;
		}
		
		public bool CanMove()
		{
			return false;
		}
	}
}
