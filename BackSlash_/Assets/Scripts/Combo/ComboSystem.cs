using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static ComboDatabase;

public class ComboSystem : MonoBehaviour
{

    [Header("Combos")]
    public ComboDatabase ComboDatabase;

    [Header("Settings")]
    [SerializeField] private float _inputBufferTime = 1f; 
    [SerializeField] private float _comboDelay = 0.3f;
    [SerializeField] private float _moveDelay = 0.3f;

    [SerializeField] private List<InputActionReference> _inputBuffer = new List<InputActionReference>();

    private float _lastInputTime;
    private Animator _animator;
    private Coroutine _currentAttackRoutine;

    private bool _isAttacking = false;

    public event Action<bool> IsAttacking;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        var inputActions = ComboDatabase.GetAllUsedActionReferences();
        foreach (var inputAction in inputActions)
        {
            inputAction.action.Enable();
            inputAction.action.performed += _ => RegisterInput(inputAction);
        }
    }

    private void OnDestroy()
    {
        var inputActions = ComboDatabase.GetAllUsedActionReferences();
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
            _inputBuffer.Clear();
        }
        if (_inputBuffer.Count > 3)
        {
            _inputBuffer.Clear();
        }
        IsAttacking?.Invoke(_isAttacking);
    }

    private void RegisterInput(InputActionReference attackInput)
    {
        _lastInputTime = Time.time;
        _inputBuffer.Add(attackInput);

        Sequence matchedCombo = DetectCombo();

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

    private Sequence DetectCombo()
    {
        foreach (var combo in ComboDatabase.GetSequences())
        {
            if (_inputBuffer.Count == combo.inputActions.Length)
            {
                bool isMatch = true;
                int comboStartIndex = _inputBuffer.Count - combo.inputActions.Length;

                for (int i = 0; i < combo.inputActions.Length; i++)
                {
                    var bufferedAttack = _inputBuffer[comboStartIndex + i];
                    var comboAttack = combo.inputActions[i];

                    if (bufferedAttack.action != comboAttack.action)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    _inputBuffer.Clear();
                    return combo;
                }
            }
        }
        return null;
    }

    private IEnumerator PerformCombo(Sequence combo)
    {
        _isAttacking = true;    
        _animator.SetTrigger(combo.animationTrigger);
        // Debug.Log($"Performing combo: {combo.name}");
        yield return new WaitForSeconds(_comboDelay);

        _isAttacking = false;
    }

    private IEnumerator PerformSimpleMove(InputActionReference input)
    {
        if (input.action.name.Contains("Attack"))
        {
            _isAttacking = true;
            _animator.SetTrigger(input.action.name);
        }

        //Debug.Log($"Performing move: {input.action.name}");
        yield return new WaitForSeconds(_moveDelay);

        _isAttacking = false;
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
