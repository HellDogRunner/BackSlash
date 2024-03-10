using Scripts.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Scripts.Player.Attack
{
    public class PlayerAttackService : MonoBehaviour
    {
        [SerializeField] Collider DamageCollider;

        private InputService _inputService;
        private HealhService _healhService;

        [Inject]
        private void Construct(InputService inputService, HealhService healhService)
        {
            _healhService = healhService;
            _inputService = inputService;


            _inputService.OnLightAttackPressed += LightAttack;
            _inputService.OnHardAttackPressed += HardAttack;
        }


        private void LightAttack() 
        {
             //_healhService.TakeDamage(5);
        }

        private void HardAttack()
        {
        }
    }
}
