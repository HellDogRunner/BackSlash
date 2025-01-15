namespace Scripts.Player
{
	public class AttackState : BasicState, IPlayerState
	{
		public AttackState(PlayerStateController player)
		{
			_player = player;
		}

		public void Enter()
		{
			_isActive = true;
			_player.ComboSystem.IsAttacking += Attack;
			_player.State = EPlayerState.Attack;
			_player.Animator.Attack(true);
		}
		
		public void Update() 
		{
			AirCheck();
			if (!_isActive) _player.SetState(new NoneState(_player));
			// TODO Start moving before the attack ends?
			// take the animation time from combo system?
		}

		public void Exit()
		{
			_player.Animator.Attack(false);
			_player.ComboSystem.IsAttacking -= Attack;
		}
		
		private void Attack(bool value)
		{
			if (!value)
			{
				_interruptible = true;
				_player.SetState(new NoneState(_player));
			}
		}
		
		public bool CanEnterInAir()
		{
			return true;
		}

		public bool CanJump()
		{
			return false;
		}
				
		public bool CanMove()
		{
			return false;
		}
	}
}
