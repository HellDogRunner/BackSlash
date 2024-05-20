using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Enemy
{
    public class EnemyAttackState : BaseEnemyState
    {
        private RaycastWeapon _weapon;
        private HealthService _health;

        private Transform _player;

        private bool _isAttack;
        private Vector3 _offset;

        private float _distanceToTarget = Mathf.Infinity;
        private float _provokedRange = 8f;
   
        public override void EnterState(EnemyStateManager enemy)
        {
            _weapon = enemy.GetComponentInChildren<RaycastWeapon>();
            _player = enemy.PlayerTransform;
            _offset = new Vector3(0, 1.3f, 0);
            _health = enemy.EnemyHealth;
        }

        public override void UpdateState(EnemyStateManager enemy)
        {
            _distanceToTarget = Vector3.Distance(_player.position, enemy.transform.position);

            enemy.gameObject.transform.LookAt(_player);
            enemy.Animator.SetFloat("Speed", enemy.Agent.velocity.magnitude);
            if (!_isAttack)
            {
                enemy.Animator.SetBool("Attack", true);
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
                enemy.Animator.SetBool("Attack", false);
            }
            if (_health.Health <= 0)
            {
                enemy.SwitchState(enemy.DeadState);
            }
        }
    }
}
