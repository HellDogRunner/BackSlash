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
			_player.State = EPlayerState.Interact;
			_player.HUD.Interact();
			_player.Movement.SetCanFall(true);
			_player.Animator.SetCanMove(false);
			_player.Movement.SetCanJump(false);
			_player.Camera.SetCanRotate(false);
			_player.Movement.SetCanSprint(false);
			// send start interact
		}

		public void Update()
		{
			AirCheck();
		}

		public void Exit()
		{
			_player.Animator.SetCanMove(true);
			_player.Camera.SetCanRotate(true);
			
			// send end interact
		}
		
		public bool CanEnterInAir()
		{
			return !_player.Movement.Air;
		}
	}
}
