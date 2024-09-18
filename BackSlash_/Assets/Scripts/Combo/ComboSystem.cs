using Scripts.Combo.Models;
using Scripts.InputReference.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

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
    private ComboInputsDatabase _inputData;

    private bool _ñomboProgress = false;
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
    public event Action<ComboInputTypeModel> OnCanAttack;
    public event Action OnCannotAttack;

    public float CancelDelay => _cancelDelay;

    [Inject]
    private void Construct(ComboDatabase comboDatabase, ComboInputsDatabase inputData)
    {
        _comboData = comboDatabase;
        _inputData = inputData;
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
                _attackInterval = StartCoroutine(ÂufferCannotExpand(combo, _inputBuffer[attackIndex]));

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
        _ñomboProgress = true;
        IsAttacking?.Invoke(true);
        _animator.SetTrigger(combo.AnimationTrigger);
        OnCombo.Invoke(combo.ComboName);

        yield return new WaitForSeconds(combo.AfterComboInterval);

        IsAttacking?.Invoke(false);

        ClearInputBuffer();
    }

    private IEnumerator PerformSimpleMove(InputActionReference inputReference)
    {
        // Ïðûæîê áëî÷èòñÿ â íà÷àëå àíèìàöèè

        string inputName = inputReference.action.name;
        ComboInputTypeModel input = _inputData.GetAnimationTypeByName(inputName);

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
        if (!_isCanceling && !_ñomboProgress)
        {
            _isCanceling = true;
            _canAttack = false;

            OnComboCancelled?.Invoke();

            yield return new WaitForSeconds(_cancelDelay);

            ClearInputBuffer();

            _isCanceling = false;
        }
    }

    private IEnumerator ÂufferCannotExpand(ComboTypeModel combo, InputActionReference inputReference)
    {
        ComboInputTypeModel input = _inputData.GetAnimationTypeByName(inputReference.action.name);

        _canAttack = false;
        yield return new WaitForSeconds(input.BeforeAttackTime);

        _attackInterval = StartCoroutine(ÂufferCanExpand(input));
    }

    private IEnumerator ÂufferCanExpand(ComboInputTypeModel input)
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
        _ñomboProgress = false;
    }

    //Ïðîâåðêà àíèìàöèîííîãî êëèïà î÷èñòèòü åñëè â äàëüíåéøåì íå ïðèãîäèòñÿ
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
