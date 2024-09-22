using Scripts.Combo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static Scripts.Combo.Models.ComboDatabase;

public class ComboSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _cancelDelay = 1f;

    [SerializeField] private List<InputActionReference> _inputBuffer = new List<InputActionReference>();

    private List<ComboTypeModel> _matchingCombos = new List<ComboTypeModel>();

    private Coroutine _currentAttackRoutine;
    private Coroutine _attackInterval;

    private Animator _animator;
    private ComboDatabase _comboData;

    private bool _�omboProgress = false;
    private bool _isCanceling = false;
    private bool _canAttack = true;

    public event Action<bool> IsAttacking;
    public event Action<string> OnAttack;
    public event Action<string> OnCombo;
    public event Action<ComboTypeModel, InputAction> OnNextAttackMatched;
    public event Action<ComboTypeModel> OnAttackMatched;
    public event Action<ComboTypeModel> OnAttackNotMatched;
    public event Action<ComboTypeModel> OnComboFinished;
    public event Action OnComboCancelled;
    public event Action OnStopAllCombos;
    public event Action<InputActionSettings> OnCanAttack;
    public event Action OnCannotAttack;

    public float CancelDelay => _cancelDelay;

    [Inject]
    private void Construct(ComboDatabase comboDatabase)
    {
        _comboData = comboDatabase;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        FillComboList();
        TryGetNextAttack(null, 0);
    }

    private void OnEnable()
    {
        var inputActions = _comboData.GetAllUsedActionReferences();
        foreach (var inputAction in inputActions)
        {
            inputAction.action.Enable();
            inputAction.action.performed += _ => RegisterInput(inputAction);
        }
    }

    private void OnDestroy()
    {
        var inputActions = _comboData.GetAllUsedActionReferences();
        foreach (var inputAction in inputActions)
        {
            inputAction.action.Disable();
            inputAction.action.performed -= _ => RegisterInput(inputAction);
        }
    }

    private void RegisterInput(InputActionReference attackInput)
    {
        if (_attackInterval != null) StopCoroutine(_attackInterval);

        if (_canAttack)
        {
            _inputBuffer.Add(attackInput);
            ComboTypeModel matchedCombo = DetectCombo();

            if (matchedCombo != null)
            {
                _currentAttackRoutine = StartCoroutine(PerformCombo(matchedCombo));
            }
            else
            {
                _currentAttackRoutine = StartCoroutine(PerformSimpleMove(attackInput));
            }
        }
        else
        {
            StartCoroutine(CancelCombo());
        }
    }

    private ComboTypeModel DetectCombo()
    {
        int attackCount = _inputBuffer.Count;
        int attackIndex = _inputBuffer.Count - 1;
        ComboTypeModel isCombo = null;

        if (attackIndex < 0) return null;

        foreach (var combo in _comboData.GetData())
        {
            if (attackCount > combo.InputActions.Length) continue;

            InputAction action = combo.InputActions[attackIndex].action;

            if (action == _inputBuffer[attackIndex].action && _matchingCombos.Contains(combo))
            {
                if (_currentAttackRoutine != null) StopCoroutine(_currentAttackRoutine);
                if (_attackInterval != null) StopCoroutine(_attackInterval);
                _attackInterval = StartCoroutine(�ufferCannotExpand(combo, _inputBuffer[attackIndex]));

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
        _�omboProgress = true;
        IsAttacking?.Invoke(true);
        _animator.SetTrigger(combo.AnimationTrigger);
        OnCombo.Invoke(combo.ComboName);

        yield return new WaitForSeconds(combo.AfterComboInterval);

        IsAttacking?.Invoke(false);

        ClearInputBuffer();
    }

    private IEnumerator PerformSimpleMove(InputActionReference inputReference)
    {
        // ������ �������� � ������ ��������

        string inputName = inputReference.action.name;
        InputActionSettings input = _comboData.GetInputActionSettingByName(inputName);

        IsAttacking?.Invoke(true);
        _animator.SetTrigger(inputName);
    
        OnAttack.Invoke(inputName);

        //if (input.action.name.Contains("Attack"))
        //{
        //    IsAttacking?.Invoke(true);
        //    _animator.SetTrigger(input.action.name);
        //    OnAttack.Invoke(input.action.name);
        //}

        yield return new WaitForSeconds(input.Length);
        IsAttacking?.Invoke(false);
    }

    private IEnumerator CancelCombo()
    {
        if (!_isCanceling && !_�omboProgress)
        {
            _isCanceling = true;
            _canAttack = false;

            OnComboCancelled?.Invoke();

            yield return new WaitForSeconds(_cancelDelay);

            ClearInputBuffer();

            _isCanceling = false;
        }
    }

    private IEnumerator �ufferCannotExpand(ComboTypeModel combo, InputActionReference inputReference)
    {
        InputActionSettings input = _comboData.GetInputActionSettingByName(inputReference.action.name);


        _canAttack = false;
        yield return new WaitForSeconds(input.BeforeAttackTime);

        _attackInterval = StartCoroutine(�ufferCanExpand(input));
    }

    private IEnumerator �ufferCanExpand(InputActionSettings input)
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
            foreach (var _combo in _comboData.GetData())
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

        foreach (var combo in _comboData.GetData())
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
        _�omboProgress = false;
    }

    //�������� ������������� ����� �������� ���� � ���������� �� ����������
    //private float GetAnimationLength(string animationTrigger)
    //{
    //    AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
    //    foreach (var clip in clips)
    //    {
    //        if (clip.name == animationTrigger)
    //        {
    //            return clip.length;
    //        }
    //    }
    //    return 0.5f;
    //}
}
