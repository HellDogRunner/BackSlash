using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Scripts.Enemy
{
    public abstract class BaseEnemyState
    {
        public abstract void EnterState(EnemyStateManager enemy, Transform Target = null);
        public abstract void UpdateState(EnemyStateManager enemy);

    }
}