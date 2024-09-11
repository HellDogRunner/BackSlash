using RedMoonGames.Database;
using System;
using UnityEngine.InputSystem;

namespace Scripts.Combo.Models
{
    [Serializable]
    public class ComboTypeModel : IDatabaseModelPrimaryKey<string>
    {
        public string ComboName;
        public InputActionReference[] InputActions;
        public float MaxInputTime;
        public string AnimationTrigger;

        public string PrimaryKey => ComboName;
    }
}