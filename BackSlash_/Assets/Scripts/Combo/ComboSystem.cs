using Scripts.Animations;
using Scripts.Combo.Models;
using Scripts.Player;
using Scripts.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ComboSystem : MonoBehaviour
{
	[SerializeField] private List<InputActionReference> _inputBuffer = new List<InputActionReference>();

	private float _cancelDelay;

	private List<ComboTypeModel> _matchingCombos = new List<ComboTypeModel>();
	private Dictionary<InputActionReference, Action<InputAction.CallbackContext>> _callbackInputContexts = new Dictionary<InputActionReference, Action<InputAction.CallbackContext>>();
	private Coroutine _currentAttackRoutine;
	private Coroutine _attackInterval;

	private ComboDatabase _comboData;
	private WeaponController _weaponController;
	private PlayerAnimationController _playerAnimationController;
	private PlayerStateController _stateController;

	private bool _ComboProgress = false;
	private bool _isCanceling = false;
	private bool _canAttack = true;

	public event Action<bool> IsAttacking;
	public event Action OnAttackSound;
	public event Action OnComboSound;
	public event Action<ComboTypeModel, InputAction> OnNextAttackMatched;
	public event Action<ComboTypeModel> OnAttackMatched;
	public event Action<ComboTypeModel> OnAttackNotMatched;
	public event Action<ComboTypeModel> OnComboFinished;
	public event Action<ComboInputSettingsModel> OnCanAttack;
	public event Action OnComboCancelled;
	public event Action OnStopAllCombos;
	public event Action OnCannotAttack;

	[Inject]
	private void Construct(PlayerStateController stateController, ComboDatabase comboDatabase, WeaponController weaponController, PlayerAnimationController playerAnimationController)
	{
		_stateController = stateController;
		_comboData = comboDatabase;
		_weaponController = weaponController;
		_playerAnimationController = playerAnimationController;
	}

	private void Awake()
	{
		_cancelDelay = _comboData.GetCancelDelay();

		FillComboList();
		TryGetNextAttack(null, 0);
	}

	private void OnEnable()
	{
		var inputActions = _comboData.GetAllUsedActionReferences();
		foreach (var inputAction in inputActions)
		{
			inputAction.action.Enable();
			Action<InputAction.CallbackContext> registerAction = _ => RegisterInput(inputAction);
			inputAction.action.performed += registerAction.Invoke;
			_callbackInputContexts.Add(inputAction, registerAction);
		}
	}

	private void OnDestroy()
	{
		var inputActions = _comboData.GetAllUsedActionReferences();
		foreach (var inputAction in inputActions)
		{
			inputAction.action.Disable();
			if (_callbackInputContexts.TryGetValue(inputAction, out var actionProccesor))
			{
				inputAction.action.performed -= actionProccesor.Invoke;
			}
		}
		_callbackInputContexts.Clear();
	}

	private void RegisterInput(InputActionReference attackInput)
	{
		if (_weaponController.CurrentWeaponType != EWeaponType.None && Time.timeScale == 1 && _stateController.CanAttack())
		{
			if (_attackInterval != null) StopCoroutine(_attackInterval);

			if (_canAttack)
			{
				_inputBuffer.Add(attackInput);
				ComboTypeModel matchedCombo = DetectCombo();

				if (matchedCombo != null) _currentAttackRoutine = StartCoroutine(PerformCombo(matchedCombo));
				else _currentAttackRoutine = StartCoroutine(PerformSimpleMove(attackInput));
			}
			
			else StartCoroutine(CancelCombo());
		}
	}

	private ComboTypeModel DetectCombo()
	{
		int attackCount = _inputBuffer.Count;
		int attackIndex = _inputBuffer.Count - 1;
		ComboTypeModel isCombo = null;

		if (attackIndex < 0) return null;

		foreach (var combo in _comboData.GetSequenceData())
		{
			if (attackCount > combo.InputActions.Length) continue;

			InputAction action = combo.InputActions[attackIndex].action;

			if (action == _inputBuffer[attackIndex].action && _matchingCombos.Contains(combo))
			{
				if (_currentAttackRoutine != null) StopCoroutine(_currentAttackRoutine);
				if (_attackInterval != null) StopCoroutine(_attackInterval);
				_attackInterval = StartCoroutine(BufferCannotExpand(combo, _inputBuffer[attackIndex]));

				OnCannotAttack?.Invoke();
				OnAttackMatched?.Invoke(combo);
				TryGetNextAttack(combo, attackCount);

				if (_inputBuffer.Count == combo.InputActions.Length)
				{
					OnComboFinished?.Invoke(combo);
					StopCoroutine(_attackInterval);
					isCombo = combo;
				}
			}
			else
			{
				AttackNotMatched(combo);
			}
		}
		return isCombo;
	}

	private void AttackNotMatched(ComboTypeModel combo)
	{
		if (_matchingCombos.Contains(combo)) _matchingCombos.Remove(combo);

		if (_matchingCombos.Count == 0) StartCoroutine(CancelCombo());

		else OnAttackNotMatched?.Invoke(combo);
	}

	private IEnumerator PerformCombo(ComboTypeModel combo)
	{
		_ComboProgress = true;
		IsAttacking?.Invoke(true);
		_playerAnimationController.TriggerAnimationByName(combo.AnimationTrigger);
		OnComboSound.Invoke();

		yield return new WaitForSeconds(combo.AfterComboInterval);

		IsAttacking?.Invoke(false);

		ClearInputBuffer();
	}

	private IEnumerator PerformSimpleMove(InputActionReference inputReference)
	{
		string inputName = inputReference.action.name;
		ComboInputSettingsModel input = _comboData.GetInputActionSettingByName(inputName);

		if (!input.MovementRalated)
		{
			IsAttacking?.Invoke(true);
			_playerAnimationController.TriggerAnimationByName(inputName);
			OnAttackSound.Invoke();
		}
		yield return new WaitForSeconds(input.Length);
		IsAttacking?.Invoke(false);
	}

	private IEnumerator CancelCombo()
	{
		if (!_isCanceling && !_ComboProgress)
		{
			_isCanceling = true;
			_canAttack = false;

			OnComboCancelled?.Invoke();

			yield return new WaitForSeconds(_cancelDelay);

			ClearInputBuffer();

			_isCanceling = false;
		}
	}

	private IEnumerator BufferCannotExpand(ComboTypeModel combo, InputActionReference inputReference)
	{
		ComboInputSettingsModel input = _comboData.GetInputActionSettingByName(inputReference.action.name);

		_canAttack = false;
		yield return new WaitForSeconds(input.BeforeAttackTime);

		_attackInterval = StartCoroutine(BufferCanExpand(input));
	}

	private IEnumerator BufferCanExpand(ComboInputSettingsModel input)
	{
		_canAttack = true;
		OnCanAttack?.Invoke(input);

		yield return new WaitForSeconds(input.CanAttackTime);

		_canAttack = false;
		OnCannotAttack?.Invoke();

		StartCoroutine(CancelCombo());
	}

	private void TryGetNextAttack(ComboTypeModel combo, int attackCount)
	{
		if (combo == null)
		{
			foreach (var _combo in _comboData.GetSequenceData())
			{
				OnNextAttackMatched?.Invoke(_combo, _combo.InputActions[0]);
			}
			return;
		}

		if (attackCount < combo.InputActions.Length)
		{
			OnNextAttackMatched?.Invoke(combo, combo.InputActions[attackCount]);
		}
	}

	private void FillComboList()
	{
		_matchingCombos.Clear();

		foreach (var combo in _comboData.GetSequenceData())
		{
			_matchingCombos.Add(combo);
		}
	}

	private void ClearInputBuffer()
	{
		OnStopAllCombos?.Invoke();

		TryGetNextAttack(null, 0);
		_inputBuffer.Clear();
		FillComboList();
		_canAttack = true;
		_ComboProgress = false;
	}
}
