using Scripts.Entity;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private Entity _entity;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        _entity.OnDeath += EnableCollision;
    }

    private void OnDisable()
    {
        _entity.OnDeath -= EnableCollision;
    }

    public void AttackTaken(AttackModel attack) => _entity.HitTaken(attack);

    public GameObject GetGameObject() => _entity.gameObject;

    private void EnableCollision()
    {
        _collider.isTrigger = false;
    }
}
