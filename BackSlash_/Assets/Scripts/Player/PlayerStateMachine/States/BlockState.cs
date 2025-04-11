using Scripts.Entity;
using UnityEngine;

namespace Scripts.Player
{
	public enum EParry : int
	{
		None = 0,
		Parry = 1,
		Block = 2
	}

	public class BlockState : BasicState, IPlayerState
	{
		private float _blockReduction = 75f;
		//private float _parryReduction = 100f;
		
		private EParry _state = EParry.Block;
	
		public BlockState(PlayerStateController player)
		{
			_player = player;
		}

		public void Enter()
		{
			_player.OnCoroutineEnd += FinishParry;
		
			_isActive = true;
			_interruptible = true;
			_player.State = EPlayerState.Block;
			_player.Animator.Block(true);
			_player.Camera.SetCanRotate(true);
			_player.Movement.SetCanFall(true);
			_player.Movement.SetCanJump(true);
			_player.Movement.SetCanSprint(false);
			
			TryStartParry();
		}

		public void Update()
		{
			if (!_isActive) _player.SetState(new NoneState(_player));
			// TODO Realize block
			// block time ect.
			_player.Parry = _state;
		}

		public void Exit()
		{
			_player.OnCoroutineEnd -= FinishParry;
			
			_state = EParry.None;
			_player.Parry = _state;
			_player.StartParryDelay(); //FIXME
			_player.TryStopCoroutine(ref _player.ParryTimeRoutine);
			_player.Animator.Block(false);
		}

		public bool CanEnterInAir()
		{
			return true;
		}

        public override void HitTaken(AttackModel attack)
        {
			if (_state == EParry.Parry)
			{
				Debug.Log("parry");
				_player.TryStopCoroutine(ref _player.ParryIntervalRoutine);
				_player.CanParry = true;
			    _player.Entity.RegisterAttack(AttackParried(attack));
			}
			else if (_state == EParry.Block)
			{
			    _player.Entity.RegisterAttack(AttackBlocked(attack));
			}
        }
        
        private void FinishParry()
        {
            _state = EParry.Block;
        }
        
        //TODO block and parry indication (paticles etc.)
		// parry and block special attacks
        
        private AttackModel AttackBlocked(AttackModel attack)
        {
			var newAttack = _player.Entity.Setup.GetAttack(attack);
			
			newAttack.Damage = GetReducedValue(newAttack.Damage, _blockReduction);
			newAttack.StabilityDamage = GetReducedValue(newAttack.StabilityDamage, _blockReduction);
			newAttack.EffectValue = GetReducedValue(newAttack.EffectValue, _blockReduction);
			
            return newAttack;
        }
        
        private AttackModel AttackParried(AttackModel attack)
        {
			var newAttack = _player.Entity.Setup.GetAttack(attack);
			
			newAttack.Damage = 0;
			newAttack.StabilityDamage = 0;
			newAttack.EffectValue = 0;
			
            return newAttack;
		}
        
        private void TryStartParry()
        {
			if (!_player.CanParry) return;
        
			_state = EParry.Parry;
			// _player.StartParryDelay(); //FIXME start when block ended???
            _player.StartCoroutine(ref _player.ParryTimeRoutine, _player.ParryTime);
        }
        
        private int GetReducedValue(int value, float reduce)
        {
            return (int)(value - value * reduce / 100);
        }
	}
}
