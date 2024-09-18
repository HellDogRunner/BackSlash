using RedMoonGames.Database;
using RedMoonGames.Basics;
using UnityEngine;

namespace Scripts.InputReference.Models
{
    [CreateAssetMenu(fileName = "InputTypesDatabase", menuName = "[RMG] Scriptable/Combo/InputTypesDatabase", order = 1)]
    public class InputDatabase : ScriptableDatabase<InputTypeModel>
    {
        public InputTypeModel GetAnimationTypeByName(string inputName)
        {
            if (inputName == "")
            {
                return null;
            }

            return _data.GetBy(InputModel => InputModel.InputName == inputName);
        }
    }
}