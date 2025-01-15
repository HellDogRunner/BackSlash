namespace Scripts.Player
{
	public class InteractState : BasicState, IPlayerState
	{
		public InteractState(PlayerStateController player)
		{
			_player = player;
		}

		public void Enter()
		{
			_isActive = true;
			_player.State = EPlayerState.Interact;
			_player.Cursor.Interact(true);
			_player.HUD.Interact();
			// send start interact
		}

		public void Update()
		{
			AirCheck();
			if (!_isActive) _player.SetState(new NoneState(_player));
		}

		public void Exit()
		{
			_player.Cursor.Interact(false);
			
			// send end interact
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
