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
			_player.Movement.SetCanFall(true);
			_player.Movement.SetCanMove(false);
			_player.Movement.SetCanJump(false);
			_player.Camera.SetCanRotate(false);
			_player.Movement.SetCanSprint(false);
			// send start loot state

		}

		public void Update()
		{
			AirCheck();
			if (!_isActive) _player.SetState(new NoneState(_player));
		}

		public void Exit()
		{
			_player.Movement.SetCanMove(true);
			
			// send end loot state
		}
		
		public bool CanEnterInAir()
		{
			return !_player.Movement.Air;
		}
	}
}
