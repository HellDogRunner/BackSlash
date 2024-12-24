namespace Scripts.Player
{
	public class DodgeState : IPlayerState
	{
		protected PlayerStateController _player;
		
		public DodgeState(PlayerStateController player)
		{
			_player = player;
		}

		public bool CanEnter()
		{
			return (_player.State == EPlayerState.None || _player.State == EPlayerState.Block) && !_player.Movement.Air;
		}

		public void Enter()
		{
			_player.State = EPlayerState.Dodge;
			_player.SendDodge(true);
		}

		public void Exit()
		{
			_player.SendDodge(false);
		}
	}
}
