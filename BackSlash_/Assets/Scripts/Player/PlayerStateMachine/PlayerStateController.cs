using RedMoonGames.Window;
using Scripts.Animations;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	public class PlayerStateController : MonoBehaviour
	{
		private TargetLock _targetLock;
		private ComboSystem _comboSystem;
		private CursorController _cursor;
		private MovementController _movement;
		private HUDController _hudController;
		private InputController _inputController;
		private PlayerAnimationController _animator;

		private IPlayerState _currentState;
		
		public EPlayerState State;

		public Target TargetLock => _targetLock.Target;
		public MovementController Movement => _movement;
		public PlayerAnimationController Animator => _animator;
		public ComboSystem ComboSystem => _comboSystem;
		public CursorController Cursor => _cursor;
		public HUDController HUD => _hudController;

		[Inject]
		private void Construct(HUDController hudController, CursorController cursor, InputController inputController, ComboSystem comboSystem, PlayerAnimationController animator, TargetLock targetLock, MovementController movement)
		{
			_inputController = inputController;
			_hudController = hudController;
			_comboSystem = comboSystem;
			_targetLock = targetLock;
			_movement = movement;
			_animator = animator;
			_cursor = cursor;
		}

		private void OnEnable()
		{
			_inputController.OnDodgeKeyPressed += Dodge;
			_inputController.OnBlockPressed += Block;
			_comboSystem.IsAttacking += Attack;
		}

		private void OnDisable()
		{
			_inputController.OnDodgeKeyPressed -= Dodge;
			_inputController.OnBlockPressed -= Block;
			_comboSystem.IsAttacking -= Attack;
		}

		private void Awake()
		{
			_currentState = new NoneState(this);
			_currentState.Enter();
		}

		private void Update()
		{
			if (_currentState != null) _currentState.Update();
		}

		public void SetState(IPlayerState newState)
		{
			if (_currentState.CanBeInterrupt() && newState.CanEnterInAir())
			{
				if (_currentState != null) _currentState.Exit();
				_currentState = newState;
				_currentState.Enter();
			}
		}

		public void Attack(bool input) { if (input) SetState(new AttackState(this)); }
		public void SetInteract() { SetState(new InteractState(this)); }
		public void SetNone() { SetState(new NoneState(this)); }
		public void SetLoot() { SetState(new LootState(this)); }
		public void Dodge() { SetState(new DodgeState(this)); }
		
		public void Block(bool input)
		{
			if (input) SetState(new BlockState(this));
			else SetState(new NoneState(this));
		}

		private void CanBeInterrupt()
		{
			_currentState.SetInterruptible();
		}

		private void AnimationEnd()
		{
			_currentState.SetInactive();
		}
		
		public bool CanJump()
		{
			return _currentState.CanJump();
		}
		
		public bool CanMove()
		{
			return _currentState.CanMove();
		}
		
		///
		public bool LockedRotate()
		{
			return State == EPlayerState.None && !_movement.Air && _movement.TrySprint;
		}
		
		public bool SlowedRotate()
		{
			return State == EPlayerState.Block || State == EPlayerState.Dodge || _movement.Air;
		}
		///
		// TODO then player rotation slowed??
		
		public bool CanAttack()
		{
			return _currentState.CanBeInterrupt() || State == EPlayerState.Attack;	/// check transitions
		}
		
		public bool CanInteract()
		{
			return TargetLock == null;
		}
	}
}
