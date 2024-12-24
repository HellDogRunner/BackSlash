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

		public MovementController Movement => _movement;
		public TargetLock TargetLock => _targetLock;

		public event Action<bool> OnNone;
		public event Action<bool> OnPause;
		public event Action<bool> OnInteract;
		public event Action<bool> OnLoot;
		public event Action<bool> OnAttack;
		public event Action<bool> OnBlock;
		public event Action<bool> OnDodge;

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
				Debug.Log("EXIT => " + _currentState);
				_currentState.Exit();
				_currentState = newState;
				_currentState.Enter();
				Debug.Log("Enter => " + _currentState);
			}
		}
		
		public void SendInteract(bool invoke) { OnInteract?.Invoke(invoke); }
		public void SendAttack(bool invoke) { OnAttack?.Invoke(invoke); }
		public void SendPause(bool invoke) { OnPause?.Invoke(invoke); }
		public void SendBlock(bool invoke) { OnBlock?.Invoke(invoke); }
		public void SendDodge(bool invoke) { OnDodge?.Invoke(invoke); }
		public void SendNone(bool invoke) { OnNone?.Invoke(invoke); }
		public void SendLoot(bool invoke) { OnLoot?.Invoke(invoke); }

		public void Interact() { SetState(new InteractState(this)); }
		public void Attack() { SetState(new AttackState(this)); }
		public void Pause() { SetState(new PauseState(this)); }
		public void Dodge() { SetState(new DodgeState(this)); }
		public void Block() { SetState(new BlockState(this)); }
		public void None() { SetState(new NoneState(this)); }
		public void Loot() { SetState(new LootState(this)); }

		public bool CanJump()
		{
			return State != EPlayerState.Dodge && State != EPlayerState.Attack;
		}

		public bool CanAttack()
		{
			return State == EPlayerState.None || State == EPlayerState.Block || State == EPlayerState.Attack;
		}

		public bool CanDodge()
		{
			return State == EPlayerState.None || State == EPlayerState.Block;
		}

		public bool CanRotate()
		{
			return (State == EPlayerState.None || State == EPlayerState.Attack || State == EPlayerState.Block) && !_movement.Air;
		}
	}
}
