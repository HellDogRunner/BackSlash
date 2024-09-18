using Scripts.Combo.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class HUDComboHandler : MonoBehaviour
{
    [SerializeField] private string _comboName;

    [Header("Initialization")] 
    [SerializeField] private ComboAnimationService _comboAnimation;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _frameImage;

    [Header("Intervals")]
    [SerializeField] private float _beforeAttackTime;
    [SerializeField] private float _canAttackTime;
    [SerializeField] private float _afterComboInterval;
    [SerializeField] private float _cancelDelay;

    private ComboTypeModel _combo;

    private bool _isActive = true;

    private ComboSystem _comboSystem;
    private ComboDatabase _comboData;

    [Inject]
    private void Construct(ComboSystem comboSystem, ComboDatabase comboDatabase)
    {
        _comboData = comboDatabase;
        _combo = _comboData.GetComboTypeByName(_comboName);

        _afterComboInterval = _combo.AfterComboInterval;
        _iconImage.sprite = _combo.IconSprite;
        _frameImage.sprite = _combo.FrameSprite;

        _comboSystem = comboSystem;
        _cancelDelay = _comboSystem.CancelDelay;

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

        _comboAnimation.SetFillVolume(_combo.InputActions.Length);
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
        _comboAnimation.SetStartAnimations();
    }

    private void NextComboAttack(ComboTypeModel combo, InputAction action)
    {
        if (!CheckThisCombo(combo)) return;

        if (_isActive)
        {
            _comboAnimation.ShowNextComboKey(action);
        }
    }

    private void ComboProgress(ComboTypeModel combo)
    {
        if (!CheckThisCombo(combo)) return;

        if (_isActive)
        {
            _comboAnimation.AnimateProgressCombo();
        }
    }

    private void CancelAllCombo()
    {
        if (_isActive)
        {
            _comboAnimation.AnimateCancelCombo();
            _isActive = false;
        }
    }

    private void ComboCanceled(ComboTypeModel combo)
    {
        if (!CheckThisCombo(combo)) return;

        if (_isActive)
        {
            _comboAnimation.AnimateCancelCombo();
            _isActive = false;
        }
    }

    private void ComboFinished(ComboTypeModel combo)
    {
        if (!CheckThisCombo(combo)) return;

        _comboAnimation.AnimateFinishCombo();
    }

    private void OnDestroy()
    {
        _comboSystem.OnComboFinished -= ComboFinished;
        _comboSystem.OnAttackMatched -= ComboProgress;
        _comboSystem.OnAttackNotMatched -= ComboCanceled;
        _comboSystem.OnNextAttackMatched -= NextComboAttack;
        _comboSystem.OnComboCancelled -= CancelAllCombo;
        _comboSystem.OnStopAllCombos -= SetStartState;
    }
}