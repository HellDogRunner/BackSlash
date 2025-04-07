using Scripts.Entity;
using UnityEngine;

public class AnimatorEventHandler : MonoBehaviour
{
    private Entity _entity;
    
    private void Start()
    {
        _entity = GetComponentInParent<Entity>();
    }

    private void OnStartAttack() => _entity.StartAttack();
    private void OnEndAttack() => _entity.EndAttack();
}
