using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.Abilities.Models
{
    [Serializable]
    public class AbilitiesTypeModel : IDatabaseModelPrimaryKey<EAbilityType>
    {
        public EAbilityType AbilityType;
        [Space]
        [Header("Prefab")]
        public GameObject AbilityPrefab;

        public EAbilityType PrimaryKey => AbilityType;
    }
}


