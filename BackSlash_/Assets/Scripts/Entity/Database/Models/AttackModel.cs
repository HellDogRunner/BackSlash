using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.Entity
{
    [Serializable]
    public class AttackModel : IDatabaseModelPrimaryKey<string>
    {
        public string Name;
        public EAttackType Type;
        public bool Ranged; 
        public int Damage;
        public int StabilityDamage;
        
        [Header("Timings")]
        public float TimeAfter;
        public float Cooldown;
        public float LastUseTime;
        
        [Header("Effect")]
        public EEntity Effect;
        public int EffectValue;
        public string PrimaryKey => Name;
    }
}
