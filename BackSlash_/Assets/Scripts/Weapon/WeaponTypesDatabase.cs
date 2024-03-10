using RedMoonGames.Basics;
using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.Weapon.Models
{
    [CreateAssetMenu(fileName = "WeaponTypesDatabase", menuName = "[RMG] Scriptable/Weapon/WeaponTypesDatabase", order = 1)]
    public class WeaponTypesDatabase : ScriptableDatabase<WeaponTypeModel>
    {
        public WeaponTypeModel GetWeaponTypeModel(EWeaponType weaponType)
        {
            if (weaponType == EWeaponType.None)
            {
                return null;
            }

            return _data.GetBy(weaponModel => weaponModel.WeaponType == weaponType);
        }
    }
}
