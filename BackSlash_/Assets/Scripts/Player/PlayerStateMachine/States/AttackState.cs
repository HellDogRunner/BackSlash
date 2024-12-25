namespace Scripts.Player
{
	public class AttackState : IPlayerState
	{
		private PlayerStateController _player;
		private EPlayerState state = EPlayerState.Attack;
		
		public AttackState(PlayerStateController player){ _player = player; }
		public EPlayerState GetState() { return state; }

		public bool CanEnter()
		{
			return _player.State == state || _player.State == EPlayerState.None || _player.State == EPlayerState.Block;
		}

		public void Enter()
		{
			_player.State = state;
			_player.SendAttack(true);
		}

		public void Exit()
		{
			_player.SendAttack(false);
		}
	}
}
