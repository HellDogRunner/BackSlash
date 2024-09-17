using Scripts.Combo.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class HUDComboHandler : MonoBehaviour
{
    [SerializeField] private string _comboName;

    [Header("Initialization")] 
    [SerializeField] private ComboAnimationService _animationService;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _frameImage;

    [Header("Intervals")]
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

        _canAttackInteval = _combo.CanAttackInteval;
        _afterComboInterval = _combo.AfterComboInterval;

        _iconImage.sprite = _combo.IconSprite;
        _frameImage.sprite = _combo.FrameSprite;

        _comboSystem = comboSystem;
        _comboSystem.OnComboFinished += ComboFinished;  
        _comboSystem.OnAttackMatched += ComboProgress;
        _comboSystem.OnAttackNotMatched += ComboCanceled;
        _comboSystem.OnNextAttackMatched += NextComboAttack;
        _comboSystem.OnComboCancelled += CancelAllCombo;
        _comboSystem.OnStopAllCombos += SetStartState;
        _comboSystem.OnCanAttack += CanAttack;
    }

    private void Awake()
    {
        SetStartState();

        _animationService.SetFillVolume(_combo.InputActions.Length);
    }

    private void CanAttack(ComboTypeModel combo, bool canAttack)
    {
        if (!CheckThisCombo(combo)) return;

        _animationService.AnimateCanAttack(canAttack);
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
        _animationService.AnimateCanAttack(true);
    }

    private void NextComboAttack(ComboTypeModel combo, InputAction action)
    {
        if (!CheckThisCombo(combo)) return;

        if (_isActive)
        {
            _animationService.ShowNextComboKey(action);
        }
    }

    private void ComboProgress(ComboTypeModel combo)
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
            _animationService.AnimateCanAttack(false);
            _animationService.AnimateCancelCombo();
            _isActive = false;
        }
    }

    private void ComboCanceled(ComboTypeModel combo)
    {
        if (!CheckThisCombo(combo)) return;

        if (_isActive)
        {
            _animationService.AnimateCanAttack(false);
            _animationService.AnimateCancelCombo();
            _isActive = false;
        }
    }

    private void ComboFinished(ComboTypeModel combo)
    {
        if (!CheckThisCombo(combo)) return;

        _animationService.AnimateCanAttack(true);
        _animationService.AnimateFinishCombo();
    }

    private void OnDestroy()
    {
        _comboSystem.OnComboFinished -= ComboFinished;
        _comboSystem.OnAttackMatched -= ComboProgress;
        _comboSystem.OnAttackNotMatched -= ComboCanceled;
        _comboSystem.OnNextAttackMatched -= NextComboAttack;
        _comboSystem.OnComboCancelled -= CancelAllCombo;
        _comboSystem.OnStopAllCombos -= SetStartState;
        _comboSystem.OnCanAttack -= CanAttack;
    }
}