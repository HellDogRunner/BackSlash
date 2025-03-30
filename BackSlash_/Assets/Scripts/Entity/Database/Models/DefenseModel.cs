using System;
using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.Entity
{
    [Serializable]
    public class DefenseModel : IDatabaseModelPrimaryKey<string>
    {
        public string Name;
        public EEntity Type { get; private set; }
        [Tooltip("Percentage damage reduction")] public int Reduction;
        [Tooltip("The value for triggering the effect")] public int Accumulation;
        public int Current;
        public bool Immunity;
        
        public string PrimaryKey => Name;
        
        public void SetType(EEntity type)
        {
            Type = type;
        }
    }  
}
