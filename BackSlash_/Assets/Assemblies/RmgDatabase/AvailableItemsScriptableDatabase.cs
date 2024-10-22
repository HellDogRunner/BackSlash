using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Database
{
    [Serializable]
    [CreateAssetMenu(fileName = "AvailableItemsDatabase", menuName = "Scriptable Objects/AvailableItemsDatabase")]
    public class AvailableItemsScriprtableDatabase<Tdata> : ScriptableObject
    {
        [SerializeField] protected int _currency;
        [SerializeField] protected List<Tdata> _weaponMods;

        public int GetCurrency()
        {
            return _currency;
        }

        public void SetCurrency(int value)
        { 
            _currency = value;
        }

        public List<Tdata> GetWeaponMods()
        {
            return _weaponMods;
        }
    }
}
