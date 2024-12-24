namespace Scripts.Player
{
	public class InteractState : IPlayerState
	{
		protected PlayerStateController _player;
		
		public InteractState(PlayerStateController player)
		{
			_player = player;
		}

		public bool CanEnter()
		{
			return _player.State == EPlayerState.None && !_player.Movement.Air && _player.TargetLock.Target == null;
		}

		public void Enter()
		{
			_player.State = EPlayerState.Interact;
			_player.SendInteract(true);
		}

		public void Exit()
		{
			_player.SendInteract(false);
		}
	}
}
