using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyService : MonoBehaviour
{
    [SerializeField] private List<Target> _enemies;
    public List<Target> EnemyList => _enemies;

    [Inject]
    private void Construct()
    {
    }

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

