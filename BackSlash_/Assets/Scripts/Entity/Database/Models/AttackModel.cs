using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.Entity
{
    [Serializable]
    public class AttackModel : IDatabaseModelPrimaryKey<string>
    {
        public string Name;
        public int Damage;
        public int StabilityDamage;
        
        [Header("Effects")]
        public EEntity Effect;
        public int EffectValue;
        public string PrimaryKey => Name;
    }
}
