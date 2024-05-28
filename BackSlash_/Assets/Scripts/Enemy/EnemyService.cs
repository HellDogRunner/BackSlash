using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : MonoBehaviour
{
    [SerializeField] private List<Target> _enemies;
    public List<Target> EnemyList => _enemies;

    private void Awake()
    {
        foreach (var enemy in _enemies)
        {
            enemy.OnTargetDeath += RemoveFromEnemyList;
        }
    }

    private void RemoveFromEnemyList(Target enemy)
    {
        _enemies.Remove(enemy);
    }
}

