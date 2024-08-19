using Scripts.Player;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class HUDComboController : MonoBehaviour
{
    [SerializeField] private ComboAnimationService _animationService;
    [SerializeField] private List<string> _combo;

    [Header("Key Graphics")]
    [SerializeField] private GameObject _control;
    [SerializeField] private GameObject _leftMouse;
    [SerializeField] private GameObject _rightMouse;
    [SerializeField] private GameObject _wheelMouse;

    [Header("Keys")]
    [SerializeField] private GameObject _objectMouse;
    [SerializeField] private GameObject _objectKB;
    [SerializeField] private TMP_Text _textKB;

    private int _currentKey = 0;
    private float _currentFill;

    public event Action OnComboFinished;
    public event Action OnComboCanceled;

    private Dictionary<string, GameObject> _keysGraphics = new Dictionary<string, GameObject>();
    private Dictionary<string, string> _keysText = new Dictionary<string, string>();
    private List<string> _keysName = new List<string>();

    private InputController _inputController;

    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;
        _inputController.OnAttackPressed += LightAttack;
        _inputController.OnDodgeKeyPressed += Dodge;

        _animationService.OnAnimationFinished += SetStartState;
    }

    private void Awake()
    {
        _keysGraphics.Add("control", _control);
        _keysGraphics.Add("leftMouse", _leftMouse);
        _keysGraphics.Add("rightMouse", _rightMouse);
        _keysGraphics.Add("wheelMouse", _wheelMouse);

        _keysText.Add("LightAttack", "leftMouse");
        _keysText.Add("Dodge", "control");

        _keysName.Add("leftMouse");
        _keysName.Add("rightMouse");
        _keysName.Add("wheelMouse");

        SetStartState();
    }

    private void LightAttack()
    {
        ComboManager("LightAttack");
    }

    private void Dodge()
    {
        ComboManager("Dodge");
    }

    private void SetStartState()
    {
        Tuple<GameObject, GameObject> keyElements = GetKeyElements(_combo[0]);
        SwitchActiveGraphics(keyElements.Item1);
        _animationService.ShowKey(keyElements.Item2);

        _currentKey = 0;
        _currentFill = 0;
        _animationService.IndicatorPosition(_currentFill);
        _animationService.AnimateStartCombo();
    }

    private void ComboManager(string element)
    {
        if (_currentKey == -1) return;

        Tuple<GameObject, GameObject> keyElements = GetKeyElements(_combo[_currentKey]);
        _animationService.HideKey(keyElements.Item2);

        if (element == _combo[_currentKey])
        {
            _currentFill += 1.0f / _combo.Count;
            _animationService.IndicatorPosition(_currentFill);

            if (_currentKey == _combo.Count - 1)
            {
                _currentKey++;
                FinishCombo();
                return;
            }

            Tuple<GameObject, GameObject> nextKeyElements = GetKeyElements(_combo[_currentKey + 1]);
            SwitchActiveGraphics(nextKeyElements.Item1);
            _animationService.ShowKey(nextKeyElements.Item2);
            _currentKey++;
        }
        else
        {
            _currentKey = -1;
            _animationService.IndicatorPosition(0);
            _animationService.AnimateCancelCombo();
            OnComboCanceled?.Invoke();
        }
    }

    private void FinishCombo()
    {
        _animationService.AnimateFinishCombo();
        OnComboFinished?.Invoke();
    }

    private Tuple<GameObject, GameObject> GetKeyElements(string key)
    {
        string keyText;
        GameObject keyGraphics;
        GameObject keyObject;

        _keysText.TryGetValue(key, out keyText);
        _keysGraphics.TryGetValue(keyText, out keyGraphics);

        if (_keysName.Contains(keyText))
        {
            keyObject = _objectMouse;
        }
        else
        {
            _textKB.text = keyText;
            keyObject = _objectKB;
        }

        return Tuple.Create(keyGraphics, keyObject);
    }

    private void SwitchActiveGraphics(GameObject graphics)
    {
        foreach (KeyValuePair<string, GameObject> pair in _keysGraphics)
        {
            pair.Value.SetActive(false);
        }
        graphics.SetActive(true);
    }

    private void OnDestroy()
    {
        _inputController.OnAttackPressed -= LightAttack;
        _inputController.OnDodgeKeyPressed -= Dodge;

        _animationService.OnAnimationFinished -= SetStartState;
    }
}