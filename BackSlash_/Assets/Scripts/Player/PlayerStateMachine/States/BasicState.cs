namespace Scripts.Player
{
	public class BasicState
	{
		protected bool _interruptible;
		protected bool _isActive;
		
		protected PlayerStateController _player;
		
		public bool CanBeInterrupt()
		{
			return _interruptible;
		}
		
		public void SetInterruptible()
		{
			_interruptible = true;
		}
		
		public void SetInactive()
		{
			_isActive = false;
		}
		
		protected void AirCheck()
		{
			if (_interruptible && _player.Movement.Air)
			{
				_player.Animator.Fall();
				_player.SetState(new NoneState(_player));
			}	
		}
	}
}
