using RedMoonGames.Basics;
using RedMoonGames.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Combo.Models
{
    [CreateAssetMenu(fileName = "ComboTypesDatabase", menuName = "[RMG] Scriptable/Combo/ComboTypesDatabase", order = 1)]
    public class ComboDatabase : ScriptableDatabase<ComboTypeModel>
    {
        [Serializable]
        public struct InputActionSettings
        {
            public InputActionReference InputAction;
            public float Length;
            public float BeforeAttackTime;
            public float CanAttackTime;
        }

        [SerializeField] private List<InputActionSettings> _inputActionsSettings;

        public ComboTypeModel GetComboTypeByName(string comboName)
        {
            if (comboName == "")
            {
                return null;
            }

            return _data.GetBy(comboModel => comboModel.ComboName == comboName);
        }

        public InputActionSettings GetInputActionSettingByName(string inputName)
        {
            return _inputActionsSettings.GetBy(InputSettings => InputSettings.InputAction.action.name == inputName);
        }

        public List<InputActionReference> GetAllUsedActionReferences()
        {
            List<InputActionReference> filteredInputActions = new List<InputActionReference>();

            foreach (var sequence in _data)
            {
                foreach (var inputAction in sequence.InputActions)
                {
                    filteredInputActions.Add(inputAction);
                }
            }
            var result = filteredInputActions.Distinct().ToList();
            return result;
        }

        [ContextMenu("Init all uniqe inputs")]
        private void InitAllUniqueInputs()
        {
            _inputActionsSettings.Clear();

            var filteredInputActions = GetAllUsedActionReferences();

            foreach (var inputAction in filteredInputActions)
            {
                InputActionSettings newInputSetting = new InputActionSettings();
                newInputSetting.InputAction = inputAction;
                _inputActionsSettings.Add(newInputSetting);
            }
        }
    }
}