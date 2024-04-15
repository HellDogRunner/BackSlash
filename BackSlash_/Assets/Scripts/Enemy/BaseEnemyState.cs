using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Scripts.Enemy
{
    public abstract class BaseEnemyState
    {
        public abstract void EnterState(EnemyStateManager enemy);
        public abstract void UpdateState(EnemyStateManager enemy);
        public abstract void OnAnimationTrigger(EnemyStateManager enemy);

    }
}