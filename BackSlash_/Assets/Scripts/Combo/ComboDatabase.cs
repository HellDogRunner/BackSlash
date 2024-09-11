using RedMoonGames.Database;
using RedMoonGames.Basics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Combo.Models
{
    [CreateAssetMenu(fileName = "ComboTypesDatabase", menuName = "[RMG] Scriptable/Combo/ComboTypesDatabase", order = 1)]
    public class ComboDatabase : ScriptableDatabase<ComboTypeModel>
    {
        public ComboTypeModel GetComboTypeByName(string comboName)
        {
            if (comboName == "")
            {
                return null;
            }

            return _data.GetBy(comboModel => comboModel.ComboName == comboName);
        }

        public IEnumerable<InputActionReference> GetAllUsedActionReferences()
        {
            List<InputActionReference> filteredInputActions = new List<InputActionReference>();

            foreach (var sequence in _data)
            {
                foreach (var inputAction in sequence.InputActions)
                {
                    filteredInputActions.Add(inputAction);
                }
            }
            var result = filteredInputActions.Distinct();
            return result;
        }
    }
}