using System.Collections.Generic;
using RedMoonGames.Basics;
using UnityEngine;

namespace Scripts.Entity
{
    public class DefenseController : MonoBehaviour
    {
        [SerializeField] private List<DefenseModel> _resists;
        
        public void OnAwake(List<DefenseModel> resists)
        {
            _resists = resists;
        }
        
        public int CalculateDamage(AttackModel attack)
        {
            AccumulateEffect(attack);
            return ReducePhysicalDamage(attack.Damage);
        }
        
        private int ReducePhysicalDamage(int damage)
        {
            var physical = _resists.GetBy(model => model.Type == EEntity.Physical);
            float reduction = physical.Reduction / 100f;
            
            return (int)(damage - damage * reduction);
        }
        
        private void AccumulateEffect(AttackModel attack)
        {
            if (attack.Effect == EEntity.Physical) return;
        
            foreach (var resist in _resists)
            {
                if (resist.Type == attack.Effect)
                {
                    resist.Current += attack.EffectValue;
                    
                    if (resist.Current > resist.Accumulation)
                    {
                        resist.Current = 0;
                        TriggerNegativeEffect(resist.Type);
                    }
                    
                    break;
                }
            }
        }
        
        private void TriggerNegativeEffect(EEntity effect)
        {
            Debug.Log(effect.ToString() + " Triggered");
        }
    }
}
