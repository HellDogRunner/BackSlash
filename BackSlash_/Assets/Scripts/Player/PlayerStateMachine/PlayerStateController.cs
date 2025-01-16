using RedMoonGames.Window;
using Scripts.Animations;
using Scripts.Player.camera;
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
		private CameraController _cameraController;

		private IPlayerState _currentState;
		
		public EPlayerState State;
		
		public PlayerAnimationController Animator => _animator;
		public CameraController Camera => _cameraController;
		public MovementController Movement => _movement;
		public ComboSystem ComboSystem => _comboSystem;
		public HUDController HUD => _hudController;
		public CursorController Cursor => _cursor;

		[Inject]
		private void Construct(CameraController cameraController, HUDController hudController, CursorController cursor, InputController inputController, ComboSystem comboSystem, PlayerAnimationController animator, TargetLock targetLock, MovementController movement)
		{
			_cameraController = cameraController;
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

		private void Attack(bool input) { if (input) SetState(new AttackState(this)); }
		public void SetInteract() { SetState(new InteractState(this)); }
		public void SetNone() { SetState(new NoneState(this)); }
		public void SetLoot() { SetState(new LootState(this)); }
		private void Dodge() { SetState(new DodgeState(this)); }
		
		private void Block(bool input)
		{
			if (input) SetState(new BlockState(this));
			else SetState(new NoneState(this));
		}

		public void SetBeInterrupt()
		{
			_currentState.SetInterruptible();
		}

		private void AnimationEnd()
		{
			_currentState.SetInactive();
		}
		
		public bool SlowedRotate()
		{
			return State == EPlayerState.Block || State == EPlayerState.Dodge;
		}
		
		public bool CanAttack()
		{
			return _currentState.CanBeInterrupt() || State == EPlayerState.Attack;	/// check transitions
		}
		
		// FIXME 
		public bool CanInteract()
		{
			return _targetLock.Target == null && _currentState.CanBeInterrupt();
		}
	}
}
