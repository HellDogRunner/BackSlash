namespace Scripts.Player
{
	public class NoneState : IPlayerState
	{
		private PlayerStateController _player;
		private EPlayerState state = EPlayerState.None;
		
		public NoneState(PlayerStateController player){ _player = player; }
		public EPlayerState GetState() { return state; }

		public bool CanEnter()
		{
			return true;
		}

		public void Enter()
		{
			_player.State = state;
			_player.SendNone(true);
		}

		public void Exit()
		{
			_player.SendNone(false);
		}
		
	}
}
