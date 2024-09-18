using RedMoonGames.Database;
using RedMoonGames.Basics;
using UnityEngine;

namespace Scripts.InputReference.Models
{
    [CreateAssetMenu(fileName = "ComboInputsDatabase", menuName = "[RMG] Scriptable/Combo/ComboInputsDatabase", order = 1)]
    public class ComboInputsDatabase : ScriptableDatabase<ComboInputTypeModel>
    {
        public ComboInputTypeModel GetAnimationTypeByName(string inputName)
        {
            if (inputName == "")
            {
                return null;
            }

            return _data.GetBy(InputModel => InputModel.InputName == inputName);
        }
    }
}