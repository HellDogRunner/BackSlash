namespace Scripts.Player
{
	public class AttackState : IPlayerState
	{
		protected PlayerStateController _player;
		
		public AttackState(PlayerStateController player)
		{
			_player = player;
		}

		public bool CanEnter()
		{
			return _player.State == EPlayerState.None || _player.State == EPlayerState.Attack || _player.State == EPlayerState.Block;
		}

		public void Enter()
		{
			_player.State = EPlayerState.Attack;
			_player.SendAttack(true);
		}

		public void Exit()
		{
			_player.SendAttack(false);
		}
	}
}
