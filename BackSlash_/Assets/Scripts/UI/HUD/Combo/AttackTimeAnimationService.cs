using DG.Tweening;
using Scripts.InputReference.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AttackTimeAnimationService : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _intervalImage;

    private Tween _fill;

    private ComboSystem _comboSystem;

    [Inject]
    private void Construct(ComboSystem comboSystem)
    {
        _comboSystem = comboSystem;
        _comboSystem.OnCanAttack += AnimateCanAttackTime;
        _comboSystem.OnCannotAttack += HideStrip;
    }

    private void Awake()
    {
        SetDefaultState();
    }

    private void AnimateCanAttackTime(ComboInputTypeModel input)
    {
        _intervalImage.gameObject.SetActive(true);
        _fill = _intervalImage.DOFillAmount(0, input.CanAttackTime).SetEase(Ease.Flash);
    }

    private void HideStrip()
    {
        _fill.Kill();
        SetDefaultState();  
    }

    private void SetDefaultState()
    {
        _intervalImage.gameObject.SetActive(false);
        _intervalImage.fillAmount = 1;
    }

    private void OnDestroy()
    {
        _comboSystem.OnCanAttack -= AnimateCanAttackTime;
        _comboSystem.OnCannotAttack -= HideStrip;
    }
}
