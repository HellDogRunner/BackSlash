using Scripts.Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HUDComboController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _keys;

    private List<Transform> _images;
    private int _currentKey = 0;
    private bool isKeyboard;

    //  заменить на нормальный импорт комбо из Scriptable Objects
    private List<string> _combo = new List<string>() { "LightAttack", "LightAttack", "Dodge", "LightAttack" };

    private InputController _controller;
    private ComboAnimationService _comboAnimation;

    [Inject]
    private void Construct(InputController controller, ComboAnimationService comboAnimation)
    {
        _comboAnimation = comboAnimation;

        if (_combo[0] == "LightAttack")
        {
            isKeyboard = false;
        }
        else
        {
            isKeyboard = true;
        }

        _comboAnimation.SetStartState(_keys, isKeyboard);

        _controller = controller;
        _controller.OnAttackPressed += LightAttack;
        _controller.OnDodgeKeyPressed += Dodge;
    }

    private void LightAttack()
    {
        ComboManager("LightAttack");
    }

    private void Dodge()
    {
        ComboManager("Dodge");
    }

    private void ComboManager(string item)
    {
        if (item == _combo[_currentKey])
        {
            if (_currentKey == _keys.Count - 1)
            {
                _comboAnimation.ManageAnimation(_keys[_currentKey], null, false);
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

            _comboAnimation.ManageAnimation(_keys[_currentKey], _keys[_currentKey + 1], isKey);

            CheckIndex();
        }
        else
        {
            _comboAnimation.AnimateCancelCombo();
            _comboAnimation.SetStartState(_keys, isKeyboard);
            _currentKey = 0;
        }
    }

    private void CheckIndex()
    {
        _currentKey++;

        if (_currentKey == _keys.Count)
        {
            _currentKey = 0;
            _comboAnimation.AnimateFinishCombo();
            _comboAnimation.SetStartState(_keys, isKeyboard);
        }
    }

    private void OnDestroy()
    {
        _controller.OnAttackPressed -= LightAttack;
        _controller.OnDodgeKeyPressed -= Dodge;
    }
}