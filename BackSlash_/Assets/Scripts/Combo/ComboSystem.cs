using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Scripts.Combo.Models;

public class ComboSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _inputBufferTime = 1f; 
    [SerializeField] private float _comboDelay = 0.3f;
    [SerializeField] private float _moveDelay = 0.3f;

    [SerializeField] private List<InputActionReference> _inputBuffer = new List<InputActionReference>();

    private Animator _animator;
    private Coroutine _currentAttackRoutine;
    private ComboDatabase _comboData;

    private float _lastInputTime;
    private bool _isAttacking = false;

    public event Action<bool> IsAttacking;
    public event Action <string> OnAttack;
    public event Action <string> OnCombo;

    public event Action OnInputBufferAdded;
    public event Action OnInputBufferCleared;

    public List<InputActionReference> InputBuffer => _inputBuffer;

    [Inject]
    private void Construct(ComboDatabase comboDatabase)
    {
        _comboData = comboDatabase;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        }
        if (_inputBuffer.Count > 3)
        {
            ClearInputBuffer();
        }
        IsAttacking?.Invoke(_isAttacking);
    }

    private void RegisterInput(InputActionReference attackInput)
    {
        _lastInputTime = Time.time;
        _inputBuffer.Add(attackInput);
        OnInputBufferAdded?.Invoke();

        ComboTypeModel matchedCombo = DetectCombo();

        if (!_isAttacking)
        {
            if (matchedCombo != null)
            {
                _currentAttackRoutine = StartCoroutine(PerformCombo(matchedCombo));
            }
            else
            {
                _currentAttackRoutine = StartCoroutine(PerformSimpleMove(attackInput));
            }
        }
    }

    private ComboTypeModel DetectCombo()
    {
        foreach (var combo in _comboData.GetData())
        {
            if (_inputBuffer.Count == combo.InputActions.Length)
            {
                bool isMatch = true;
                int comboStartIndex = _inputBuffer.Count - combo.InputActions.Length;

                for (int i = 0; i < combo.InputActions.Length; i++)
                {
                    var bufferedAttack = _inputBuffer[comboStartIndex + i];
                    var comboAttack = combo.InputActions[i];

                    if (bufferedAttack.action != comboAttack.action)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    ClearInputBuffer();
                    return combo;
                }
            }
        }
        return null;
    }

    private IEnumerator PerformCombo(ComboTypeModel combo)
    {
        _isAttacking = true;    
        _animator.SetTrigger(combo.AnimationTrigger);
        OnCombo.Invoke(combo.ComboName);
        yield return new WaitForSeconds(_comboDelay);

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

    private void ClearInputBuffer()
    {
        _inputBuffer.Clear();
        OnInputBufferCleared?.Invoke();
    }

    //ѕроверка анимационного клипа очистить если в дальнейшем не пригодитс€
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
