using Scripts.Combo.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class HUDComboHandler : MonoBehaviour
{
    [SerializeField] private ComboAnimationService _animationService;
    [SerializeField] private string _comboName;
    [SerializeField] private float _beforeAttackInteval;
    [SerializeField] private float _canAttackInteval;
    [SerializeField] private float _afterComboInterval;

    private ComboTypeModel _combo;

    private bool _isActive = true;

    private ComboSystem _comboSystem;
    private ComboDatabase _comboData;

    [Inject]
    private void Construct(ComboSystem comboSystem, ComboDatabase comboDatabase)
    {
        _comboData = comboDatabase;
        _combo = _comboData.GetComboTypeByName(_comboName);
        _beforeAttackInteval = _combo.BeforeAttackInteval;
        _canAttackInteval = _combo.CanAttackInteval;
        _afterComboInterval = _combo.AfterComboInterval;

        _comboSystem = comboSystem;
        _comboSystem.OnComboFinished += ComboFinished;  
        _comboSystem.OnAttackMatched += ComboProgress;
        _comboSystem.OnAttackNotMatched += ComboCanceled;
        _comboSystem.OnNextAttackMatched += NextComboAttack;
        _comboSystem.OnComboCancelled += CancelAllCombo;
        _comboSystem.OnStopAllCombos += SetStartState;
    }

    private void Awake()
    {
        SetStartState();

        _animationService.SetFillVolume(_combo.InputActions.Length);
    }

    private bool CheckThisCombo(ComboTypeModel combo)
    {
        if (_combo == combo)
        {
            return true;
        }
        return false;
    }

    private void SetStartState()
    {
        _isActive = true;
        _animationService.SetStartAnimations();
    }

    private void NextComboAttack(ComboTypeModel combo, InputAction action)
    {
        if (!CheckThisCombo(combo)) return;

        if (_isActive)
        {
            _animationService.ShowNextComboKey(action);
        }
    }

    private void ComboProgress(ComboTypeModel combo, InputAction action)
    {
        if (!CheckThisCombo(combo)) return;

        if (_isActive)
        {
            _animationService.AnimateProgressCombo();
        }
    }

    private void CancelAllCombo()
    {
        if (_isActive)
        {
            _animationService.AnimateCancelCombo();
            _isActive = false;
        }
    }

    private void ComboCanceled(ComboTypeModel combo, InputAction action)
    {
        if (!CheckThisCombo(combo)) return;

        if (_isActive)
        {
            _animationService.AnimateCancelCombo();
            _isActive = false;
        }
    }

    private void ComboFinished(ComboTypeModel combo, InputAction action)
    {
        if (!CheckThisCombo(combo)) return;
        _animationService.AnimateFinishCombo();
    }

    private void OnDestroy()
    {
        _comboSystem.OnComboFinished -= ComboFinished;
        _comboSystem.OnAttackMatched -= ComboProgress;
        _comboSystem.OnAttackNotMatched -= ComboCanceled;
        _comboSystem.OnNextAttackMatched -= NextComboAttack;
        _comboSystem.OnComboCancelled += CancelAllCombo;
        _comboSystem.OnStopAllCombos -= SetStartState;
    }
}