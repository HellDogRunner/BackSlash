using System;
using System.Collections;
using RedMoonGames.Window;
using Scripts.Animations;
using Scripts.Entity;
using Scripts.Player.camera;
using UnityEngine;
using Zenject;

namespace Scripts.Player
{
	public class PlayerStateController : MonoBehaviour
	{
		public EPlayerState State;
		
		[field: SerializeField] public float ParryTime { get; private set; }
		[field: SerializeField] public float ParryInterval { get; private set; }
		public EParry Parry;
		public bool CanParry = true;
		
		private Entity.Entity _entity;
		private TargetLock _targetLock;
		private ComboSystem _comboSystem;
		private IPlayerState _currentState;
		private MovementController _movement;
		private HUDController _hudController;
		private InputController _inputController;
		private PlayerAnimationController _animator;
		private CameraController _cameraController;
		private PlayerSoundController _soundController;

		public Coroutine ParryTimeRoutine;
		public Coroutine ParryIntervalRoutine;

		public PlayerAnimationController Animator => _animator;
		public PlayerSoundController Sound => _soundController;
		public CameraController Camera => _cameraController;
		public MovementController Movement => _movement;
		public ComboSystem ComboSystem => _comboSystem;
		public HUDController HUD => _hudController;
		public Entity.Entity Entity => _entity;
		
		public event Action OnCoroutineEnd;
		
		[Inject]
		private void Construct(Entity.Entity entity, PlayerSoundController soundController, CameraController cameraController, HUDController hudController, InputController inputController, ComboSystem comboSystem, PlayerAnimationController animator, TargetLock targetLock, MovementController movement)
		{
			_cameraController = cameraController;
			_soundController = soundController;
			_inputController = inputController;
			_hudController = hudController;
			_comboSystem = comboSystem;
			_targetLock = targetLock;
			_movement = movement;
			_animator = animator;
			_entity = entity;
		}

		private void OnEnable()
		{
			_inputController.OnDodgeKeyPressed += Dodge;
			_inputController.OnBlockPressed += Block;
			_comboSystem.IsAttacking += Attack;
			
			_entity.OnHitTaken += HitTaken;
		}

		private void OnDisable()
		{
			_inputController.OnDodgeKeyPressed -= Dodge;
			_inputController.OnBlockPressed -= Block;
			_comboSystem.IsAttacking -= Attack;
			
			_entity.OnHitTaken -= HitTaken;
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
			if (_currentState.CanBeInterrupt() && newState.CanEnterInAir() && Time.timeScale != 0)
			{
				if (_currentState != null) _currentState.Exit();
				_currentState = newState;
				_currentState.Enter();
			}
		}
		
		public void SetInteract() { SetState(new InteractState(this)); }
		public void SetNone() { SetState(new NoneState(this)); }
		public void SetLoot() { SetState(new LootState(this)); }
		private void Dodge() { SetState(new DodgeState(this)); }

		private void Attack(bool input)
		{
			if (input) SetState(new AttackState(this));
		}
		
		private void Block(bool input)
		{
			if (input && State != EPlayerState.Block) SetState(new BlockState(this));
			else if (!input && State == EPlayerState.Block) SetState(new NoneState(this));
		}

		private void HitTaken(AttackModel attack)
		{
			_currentState.HitTaken(attack);
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
		
		public void StartCoroutine(ref Coroutine coroutine, float time)
		{
			TryStopCoroutine(ref coroutine);
			coroutine = StartCoroutine(Coroutine(time));
		}

        public void StartParryDelay()
        {
            TryStopCoroutine(ref ParryIntervalRoutine);
			ParryIntervalRoutine = StartCoroutine(ParryDelay());
        }

        public void TryStopCoroutine(ref Coroutine coroutine)
		{
		    if (coroutine != null)
		    {
		    	StopCoroutine(coroutine);
		    	coroutine = null;
		    }
		}
		
		private IEnumerator Coroutine(float time)
		{
			yield return new WaitForSeconds(time);
			OnCoroutineEnd?.Invoke();
		}
		
		private IEnumerator ParryDelay()
		{
			CanParry = false;
		    yield return new WaitForSeconds(ParryInterval);
		    CanParry = true;
		}
	}
}
