using RedMoonGames.Basics;
using RedMoonGames.Database;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Combo.Models
{
    [CreateAssetMenu(fileName = "ComboTypesDatabase", menuName = "[RMG] Scriptable/Combo/ComboTypesDatabase", order = 1)]
    public class ComboDatabase : ComboScriptableDatabase<ComboTypeModel, ComboInputSettingsModel>
    {
        public ComboTypeModel GetSequenceByName(string comboName)
        {
            if (comboName == "")
            {
                return null;
            }

            return _sequences.GetBy(comboModel => comboModel.ComboName == comboName);
        }

        public ComboInputSettingsModel GetInputActionSettingByName(string inputName)
        {
            return _inputActionSettings.GetBy(InputSettings => InputSettings.InputAction.action.name == inputName);
        }

        public List<InputActionReference> GetAllUsedActionReferences()
        {
            List<InputActionReference> filteredInputActions = new List<InputActionReference>();

            foreach (var sequence in _sequences)
            {
                foreach (var inputAction in sequence.InputActions)
                {
                    filteredInputActions.Add(inputAction);
                }
            }
            var result = filteredInputActions.Distinct().ToList();
            return result;
        }

        [ContextMenu("Init all unique inputs")]
        private void InitAllUniqueInputs()
        {
            _inputActionSettings.Clear();

            var filteredInputActions = GetAllUsedActionReferences();

            foreach (var inputAction in filteredInputActions)
            {
                ComboInputSettingsModel newInputSetting = new ComboInputSettingsModel();
                newInputSetting.InputAction = inputAction;
                _inputActionSettings.Add(newInputSetting);
            }
        }
    }
}