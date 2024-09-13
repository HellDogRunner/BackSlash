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
    [SerializeField] private float _inputBufferTime = 1f;
    [SerializeField] private float _comboDelay = 0.3f;
    [SerializeField] private float _moveDelay = 0.3f;
    [SerializeField] private float _cancelDelay = 1f;

    [SerializeField] private List<InputActionReference> _inputBuffer = new List<InputActionReference>();

    private List<ComboTypeModel> _matchingCombos = new List<ComboTypeModel>();

    private Animator _animator;
    private Coroutine _currentAttackRoutine;
    private ComboDatabase _comboData;

    private float _lastInputTime;
    private bool _isAttacking = false;
    private bool _isCanceling = false;

    public event Action<bool> IsAttacking;
    public event Action<string> OnAttack;
    public event Action<string> OnCombo;
    public event Action<ComboTypeModel, InputAction> OnNextAttackMatched;
    public event Action<ComboTypeModel, InputAction> OnAttackMatched;
    public event Action<ComboTypeModel, InputAction> OnAttackNotMatched;
    public event Action<ComboTypeModel, InputAction> OnComboFinished;
    public event Action OnComboCancelled;
    public event Action OnStopAllCombos;

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

    private void Update()
    {
        if (_inputBuffer.Count > 0 && Time.time - _lastInputTime > _inputBufferTime)
        {
            ClearInputBuffer();
            _currentAttackRoutine = StartCoroutine(CancelCombo());
        }
        if (_inputBuffer.Count > 5)
        {
            ClearInputBuffer();
            _currentAttackRoutine = StartCoroutine(CancelCombo());
        }
        IsAttacking?.Invoke(_isAttacking);
    }

    private void RegisterInput(InputActionReference attackInput)
    {
        if (!_isAttacking)
        {
            _lastInputTime = Time.time;
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
            _currentAttackRoutine = StartCoroutine(CancelCombo());
        }
    }

    private ComboTypeModel DetectCombo()
    {
        int attackCount = _inputBuffer.Count;
        int attackIndex = _inputBuffer.Count - 1;

        if (attackIndex < 0) return null;

        foreach (var combo in _comboData.GetData())
        {
            if (attackCount > combo.InputActions.Length) continue;

            InputAction action = combo.InputActions[attackIndex].action;

            if (action == _inputBuffer[attackIndex].action && _matchingCombos.Contains(combo))
            {
                OnAttackMatched?.Invoke(combo, action);

                TryGetNextAttack(combo, attackCount);

                if (_inputBuffer.Count == combo.InputActions.Length && _matchingCombos.Contains(combo))
                {
                    ClearInputBuffer();
                    OnComboFinished?.Invoke(combo, action);
                    return combo;
                }
            }
            else
            {
                OnAttackNotMatched?.Invoke(combo, action);
                AttackNotMatched(combo);
            }
        }
        return null;
    }

    private void AttackNotMatched(ComboTypeModel combo)
    {
        if (_matchingCombos.Contains(combo))
        {
            _matchingCombos.Remove(combo);
        }
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

    private IEnumerator PerformCombo(ComboTypeModel combo)
    {
        _isAttacking = true;
        _animator.SetTrigger(combo.AnimationTrigger);
        OnCombo.Invoke(combo.ComboName);
        yield return new WaitForSeconds(_comboDelay);

        OnStopAllCombos?.Invoke();
        TryGetNextAttack(null, 0);

        _isAttacking = false;
    }

    private IEnumerator PerformSimpleMove(InputActionReference input)
    {
        if (input.action.name.Contains("Attack"))
        {
            _isAttacking = true;
            _animator.SetTrigger(input.action.name);
            OnAttack.Invoke(input.action.name);
        }
        yield return new WaitForSeconds(_moveDelay);

        _isAttacking = false;
    }

    private IEnumerator CancelCombo()
    {
        if (!_isCanceling)
        {
            _isCanceling = true;
            OnComboCancelled?.Invoke();

            // Тут можно оставить ивент на анимацию отменённого комбо

            yield return new WaitForSeconds(_cancelDelay);

            OnStopAllCombos?.Invoke();
            TryGetNextAttack(null, 0);

            _isAttacking = false;
            _isCanceling = false;
        }
    }

    private void ClearInputBuffer()
    {
        _inputBuffer.Clear();
        FillComboList();
    }

    //Проверка анимационного клипа очистить если в дальнейшем не пригодится
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
