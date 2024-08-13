using Scripts.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HUDComboController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _keys;
    [SerializeField] private List<string> _combo;

    private List<Transform> _images;
    private int _currentKey = 0;
    private string _firstKey;
    private bool _isKeyboard;

    public event Action OnComboFinished;
    public event Action OnComboCanceled;

    //  заменить на нормальный импорт комбо из Scriptable Objects
    //private List<string> _combo = new List<string>() { "LightAttack", "LightAttack", "Dodge", "LightAttack" };

    private InputController _inputController;
    private ComboAnimationService _comboAnimation;

    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;
        SubscribeOnEvents();

        _comboAnimation = GetComponent<ComboAnimationService>();

        if (_combo[0] == "LightAttack")
        {
            _isKeyboard = false;
            _firstKey = "leftMouse";
        }
        else
        {
            _isKeyboard = true;
            _firstKey = "control";
        }

        _comboAnimation.InstantiateKeys();
        _comboAnimation.SetStartState(_keys, _firstKey, _isKeyboard);
    }

    private void LightAttack()
    {
        ComboManager("LightAttack", "leftMouse");
    }

    private void Dodge()
    {
        ComboManager("Dodge", "control");
    }

    private void ComboManager(string segment, string key)
    {
        if (segment == _combo[_currentKey])
        {
            if (_currentKey == _keys.Count - 1)
            {
                _comboAnimation.ManageAnimation(_keys[_currentKey], null, null, false);
                CheckIndex();
                return;
            }

            bool isKey;
            if (_combo[_currentKey + 1] == "LightAttack")
            {
                isKey = false;
            }
            else
            {
                isKey = true;
            }

            _comboAnimation.ManageAnimation(_keys[_currentKey], _keys[_currentKey + 1], key, isKey);

            CheckIndex();
        }
        else
        {
            _comboAnimation.AnimateCancelCombo();
            _comboAnimation.SetStartState(_keys, _firstKey, _isKeyboard);
            _currentKey = 0;
            OnComboCanceled?.Invoke();
            UnsubscribeOnEvents();
        }
    }

    private void CheckIndex()
    {
        _currentKey++;

        if (_currentKey == _keys.Count)
        {
            _currentKey = 0;
            _comboAnimation.AnimateFinishCombo();
            _comboAnimation.SetStartState(_keys, _firstKey, _isKeyboard);
            OnComboFinished?.Invoke();
        }
    }

    public void SubscribeOnEvents()
    {
        _inputController.OnAttackPressed += LightAttack;
        _inputController.OnDodgeKeyPressed += Dodge;
    }

    public void UnsubscribeOnEvents()
    {
        _inputController.OnAttackPressed -= LightAttack;
        _inputController.OnDodgeKeyPressed -= Dodge;
    }

    public void DisableFade()
    {
        _comboAnimation.FadeOff();
    }

    private void OnDestroy()
    {
        UnsubscribeOnEvents();
    }
}