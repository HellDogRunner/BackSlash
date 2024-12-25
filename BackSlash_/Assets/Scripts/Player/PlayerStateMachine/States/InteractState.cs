namespace Scripts.Player
{
	public class InteractState : IPlayerState
	{
		private PlayerStateController _player;
		private EPlayerState state = EPlayerState.Interact;
		
		public InteractState(PlayerStateController player){ _player = player; }
		public EPlayerState GetState() { return state; }

		public bool CanEnter()
		{
			return _player.State == EPlayerState.None && _player.TargetLock == null;
		}

		public void Enter()
		{
			_player.State = state;
			_player.SendInteract(true);
		}

		public void Exit()
		{
			_player.SendInteract(false);
		}
	}
}
