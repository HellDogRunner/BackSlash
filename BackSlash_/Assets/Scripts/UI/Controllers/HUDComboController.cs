using Scripts.Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HUDComboController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _keys;

    private List<Transform> _images;
    private int _currentKey = 0;

    private InputController _controller;
    private ComboAnimationService _comboAnimation;

    [Inject]
    private void Construct(InputController controller, ComboAnimationService comboAnimation)
    {
        _comboAnimation = comboAnimation;
        _comboAnimation.SetDefaultState(_keys);

        _controller = controller;
        _controller.OnAttackPressed += Attack;
        _controller.OnDodgeKeyPressed += Dodge;
    }

    private void Attack() 
    {
        _comboAnimation.ManageAnimation(_keys[_currentKey], false);
        CheckIndex();
    }

    private void Dodge()
    {
        _comboAnimation.ManageAnimation(_keys[_currentKey], true);
        CheckIndex();
    }

    private void CheckIndex()
    {
        _currentKey++;
        if (_currentKey >= _keys.Count)
        {
            _currentKey = 0;
            _comboAnimation.AnimateFinishCombo();
            _comboAnimation.SetDefaultState(_keys);
        }
    }
}