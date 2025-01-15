namespace Scripts.Player
{
	public class DodgeState : BasicState, IPlayerState
	{
		public DodgeState(PlayerStateController player)
		{
			_player = player;
		}

		public void Enter()
		{
			_isActive = true;
			_player.State = EPlayerState.Dodge;
			_player.Animator.Dodge();
		}

		public void Update()
		{
			AirCheck();
			if (!_isActive) _player.SetState(new NoneState(_player));
			
			// TODO Realize Doddge
			// save frames etc.
		}

		public void Exit()
		{
			//_player.Animator.Dodge(false);
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
			return true;
		}
	}
}
