using Scripts.Combo.Models;
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

    private AnimationClip[] _clips;

    private Animator _animator;
    private ComboDatabase _comboData;

    private float _attackDelay;
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
    public event Action<ComboTypeModel, bool> OnCanAttack;

    [Inject]
    private void Construct(ComboDatabase comboDatabase)
    {
        _comboData = comboDatabase;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _clips = _animator.runtimeAnimatorController.animationClips;

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
            string clipName = GetClipName(attackInput);
            _inputBuffer.Add(attackInput);
            ComboTypeModel matchedCombo = DetectCombo();

            if (matchedCombo != null)
            {
                _currentAttackRoutine = StartCoroutine(PerformCombo(matchedCombo));
            }
            else
            {
                _currentAttackRoutine = StartCoroutine(PerformSimpleMove(clipName));
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
                if (_attackInterval != null) StopCoroutine(_attackInterval);
                _attackInterval = StartCoroutine(ÂufferCannotExpand(combo));
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

    private void TryGetNextAttack(ComboTypeModel combo, int attackCount)
    {
        if (combo == null)
        {
            foreach (var cmb in _comboData.GetData())
            {
                OnNextAttackMatched?.Invoke(cmb, cmb.InputActions[0]);
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

    private string GetClipName(InputActionReference input)
    {
        foreach (var clip in _clips)
        {
            if (input.action.name.Contains(clip.name))
            {
                _attackDelay = clip.length;
            }
        }

        return input.action.name;
    }

    private IEnumerator PerformCombo(ComboTypeModel combo)
    {
        _ñomboProgress = true;
        IsAttacking?.Invoke(true);

        _animator.SetTrigger(combo.AnimationTrigger);
        OnCombo.Invoke(combo.ComboName);

        yield return new WaitForSeconds(combo.AfterComboInterval);

        ClearInputBuffer();
    }

    private IEnumerator PerformSimpleMove(string inputName)
    {
        // Ïðûæîê áëî÷èòñÿ â íà÷àëå àíèìàöèè
        IsAttacking?.Invoke(true);
        _animator.SetTrigger(inputName);
        OnAttack.Invoke(inputName);

        //if (input.action.name.Contains("Attack"))
        //{
        //    IsAttacking?.Invoke(true);
        //    _animator.SetTrigger(input.action.name);
        //    OnAttack.Invoke(input.action.name);
        //}

        yield return new WaitForSeconds(_attackDelay);

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

    private IEnumerator ÂufferCannotExpand(ComboTypeModel combo)
    {
        _canAttack = false;
        OnCanAttack?.Invoke(combo, false);
        yield return new WaitForSeconds(_attackDelay);

        _attackInterval = StartCoroutine(ÂufferCanExpand(combo));
    }

    private IEnumerator ÂufferCanExpand(ComboTypeModel combo)
    {
        _canAttack = true;
        OnCanAttack?.Invoke(combo, true);
        yield return new WaitForSeconds(combo.CanAttackInteval);

        _canAttack = false;

        AttackNotMatched(combo);
    }

    private void ClearInputBuffer()
    {
        OnStopAllCombos?.Invoke();

        TryGetNextAttack(null, 0);
        _inputBuffer.Clear();
        FillComboList();

        IsAttacking?.Invoke(false);
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
