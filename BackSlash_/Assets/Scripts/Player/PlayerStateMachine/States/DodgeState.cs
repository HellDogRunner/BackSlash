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
			_player.Camera.SetCanRotate(true);
			_player.Movement.SetCanFall(false);
			_player.Movement.SetCanJump(false);
			_player.Movement.SetCanSprint(false);
			//_player.Sound.PlayRollSound();
		}

		public void Update()
		{
			AirCheck();
			if (!_isActive) _player.SetState(new NoneState(_player));
			
			// TODO Realize Dodge
			// save frames etc.
		}

		public void Exit()
		{
			
		}

		public bool CanEnterInAir()
		{
			return !_player.Movement.Air;
		}
	}
}
