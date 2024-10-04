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

    private ComboTypeModel _comboModel;

    private bool _isActive = true;

    private ComboSystem _comboSystem;
    private ComboDatabase _comboData;

    [Inject]
    private void Construct(ComboSystem comboSystem, ComboDatabase comboDatabase)
    {
        _comboData = comboDatabase;
        _comboModel = _comboData.GetSequenceByName(_comboName);

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
        _cancelDelay = _comboData.GetCancelDelay();

        _afterComboInterval = _comboModel.AfterComboInterval;
        _iconImage.sprite = _comboModel.IconSprite;
        _frameImage.sprite = _comboModel.FrameSprite;

        SetStartState();

        _comboAnimation.SetFillVolume(_comboModel.InputActions.Length);
    }

    private bool CheckThisCombo(ComboTypeModel combo)
    {
        if (_comboModel == combo)
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
            _comboAnimation.FillIndicator();
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