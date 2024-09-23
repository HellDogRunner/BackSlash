using RedMoonGames.Database;
using System;
using UnityEngine.InputSystem;

namespace Scripts.Combo.Models
{
    [Serializable]
    public class ComboInputSettingsModel : IDatabaseModelPrimaryKey<string>
    {
        public InputActionReference InputAction;
        public float Length;
        public float BeforeAttackTime;
        public float CanAttackTime;

        public string PrimaryKey => InputAction.action.name;
    }
}
