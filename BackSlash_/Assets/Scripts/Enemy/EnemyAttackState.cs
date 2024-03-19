using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemy
{
    public class EnemyAttackState : BaseEnemyState
    {

        private RaycastWeapon _weapon;
        private Transform _player;
        private bool _isAttack;
        private Vector3 _offset;
        private float _distanceToTarget = Mathf.Infinity;
        private float _provokedRange = 8f;
        public override void EnterState(EnemyStateManager enemy, Transform Target)
        {
            _weapon = enemy.GetComponentInChildren<RaycastWeapon>();
            _player = Target;
            _offset = new Vector3(0, 1.5f, 0);
        }

        public override void UpdateState(EnemyStateManager enemy)
        {
            _distanceToTarget = Vector3.Distance(_player.position, enemy.transform.position);
            if (!_isAttack)
            {
                _weapon.StartFiring();
                _isAttack = true;
            }
            if (_weapon.IsFiring)
            {
                _weapon.UpdateFiring(Time.deltaTime, _player.position + _offset);
            }
            _weapon.UpdateBullets(Time.deltaTime);
            if (_distanceToTarget > _provokedRange)
            {
                enemy.SwitchState(enemy.ChaseState);
                _weapon.StopFiring();
                _isAttack = false;
            }
            //if (_isAttack)
            //{
            //    _weapon.StopFiring();
            //    _isAttack = false;
            //}
        }
    }
}
