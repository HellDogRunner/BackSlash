using System;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	public class PlayerStateController : MonoBehaviour
	{
		private TargetLock _targetLock;
		private MovementController _movement;

		private IPlayerState _currentState;
		public EPlayerState State;

		public Target TargetLock => _targetLock.Target;

		public event Action<bool> OnNone;
		public event Action<bool> OnInteract;
		public event Action<bool> OnLoot;
		public event Action<bool> OnAttack;
		public event Action<bool> OnBlock;
		public event Action OnDodge;

		[Inject]
		private void Construct(TargetLock targetLock, MovementController movement)
		{
			_targetLock = targetLock;
			_movement = movement;
		}

		private void Awake()
		{
			_currentState = new NoneState(this);
		}

		public void SetState(IPlayerState newState)
		{
			if (_currentState != null && newState.CanEnter())
			{
				if (newState.GetState() != _currentState.GetState())
				{
					//Debug.Log(_currentState + " => " + newState);
					_currentState.Exit();
					_currentState = newState;
				}
				_currentState.Enter();
				//Debug.Log("ENTER => " + _currentState);
			}
		}
		
		public void SendInteract(bool invoke) { OnInteract?.Invoke(invoke); }
		public void SendAttack(bool invoke) { OnAttack?.Invoke(invoke); }
		public void SendBlock(bool invoke) { OnBlock?.Invoke(invoke); }
		public void SendNone(bool invoke) { OnNone?.Invoke(invoke); }
		public void SendLoot(bool invoke) { OnLoot?.Invoke(invoke); }
		public void SendDodge() { OnDodge?.Invoke(); }

		public void SetInteract() { SetState(new InteractState(this)); }
		public void SetAttack() { SetState(new AttackState(this)); }
		public void SetDodge() { SetState(new DodgeState(this)); }
		public void SetBlock() { SetState(new BlockState(this)); }
		public void SetNone() { SetState(new NoneState(this)); }
		public void SetLoot() { SetState(new LootState(this)); }

		public bool CanJump()
		{
			return State != EPlayerState.Dodge && State != EPlayerState.Attack;
		}
		
		// TODO can player rotate in dodge state?
		// maybe player can rotate in dodge state after small delay?
		public bool CanRotate()
		{
			return (State == EPlayerState.None || State == EPlayerState.Block || State == EPlayerState.Dodge) && !_movement.Air;
		}
		
		public bool CanAttack()
		{
			return new AttackState(this).CanEnter();
		}
		
		public bool CanInteract()
		{
			return State == EPlayerState.None && !_movement.Air && TargetLock == null;
		}
	}
}
