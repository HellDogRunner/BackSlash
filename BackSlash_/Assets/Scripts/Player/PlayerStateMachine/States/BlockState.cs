namespace Scripts.Player
{
	public class BlockState : BasicState, IPlayerState
	{
		public BlockState(PlayerStateController player)
		{
			_player = player;
		}

		public void Enter()
		{
			_isActive = true;
			_interruptible = true;
			_player.State = EPlayerState.Block;
			_player.Animator.Block(true);
			_player.Camera.SetCanRotate(true);
			_player.Movement.SetCanFall(true);
			_player.Movement.SetCanJump(true);
			_player.Movement.SetCanSprint(false);
		}

		public void Update()
		{
			if (!_isActive) _player.SetState(new NoneState(_player));
			// TODO Realize block
			// block time ect.
		}

		public void Exit()
		{
			_player.Animator.Block(false);
		}

		public bool CanEnterInAir()
		{
			return true;
		}
	}
}
