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
			_player.ComboSystem.IsAttacking += Attack;
			
			_isActive = true;
			_player.State = EPlayerState.Attack;
			_player.Movement.SetCanFall(false);
			_player.Movement.SetCanJump(false);
			_player.Animator.SetCanMove(false);
			_player.Camera.SetCanRotate(false);
			_player.Movement.SetCanSprint(false);
			_player.Camera.SetAttack(true);
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
			_player.ComboSystem.IsAttacking -= Attack;
			
			_player.Camera.SetAttack(false);
			_player.Animator.Attack(false);
			_player.Animator.SetCanMove(true);
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
	}
}
