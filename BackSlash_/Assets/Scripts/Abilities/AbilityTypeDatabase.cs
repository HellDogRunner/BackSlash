using RedMoonGames.Database;
using RedMoonGames.Basics;
using UnityEngine;

namespace Scripts.Abilities.Models
{
    [CreateAssetMenu(fileName = "AbilitiesTypesDatabase", menuName = "[RMG] Scriptable/Ability/AbilitiesTypesDatabase", order = 1)]
    public class AbilitiesTypesDatabase : ScriprtableDatabase<AbilitiesTypeModel>
    {
        public AbilitiesTypeModel GetAbilityTypeModel(EAbilityType abilityType)
        {
            if (abilityType == EAbilityType.None)
            {
                return null;
            }

            return _data.GetBy(abilityModel => abilityModel.AbilityType == abilityType);
        }
    }
}
