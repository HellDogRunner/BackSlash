using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.Weapon.Models
{
    [Serializable]
    public class WeaponTypeModel : IDatabaseModelPrimaryKey<EWeaponType>
    {
        public EWeaponType WeaponType;
        [Space]
        [Header("Prefab")]
        public GameObject WeaponPrefab;
        [Header("Settings")]
        public int LightAttackDamage;
        public int HardAttackDamage;
        public float AttackDistance;

        public EWeaponType PrimaryKey => WeaponType;
    }
}