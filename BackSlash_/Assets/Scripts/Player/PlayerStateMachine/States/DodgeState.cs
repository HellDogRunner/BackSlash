namespace Scripts.Player
{
	public class DodgeState : IPlayerState
	{
		private PlayerStateController _player;
		private EPlayerState state = EPlayerState.Dodge;
		
		public DodgeState(PlayerStateController player){ _player = player; }
		public EPlayerState GetState() { return state; }
		
		public bool CanEnter()
		{
			return _player.State == state || _player.State == EPlayerState.None || _player.State == EPlayerState.Block;
		}

		public void Enter()
		{
			_player.State = state;
			_player.SendDodge();
		}

		public void Exit()
		{
			
		}
	}
}
