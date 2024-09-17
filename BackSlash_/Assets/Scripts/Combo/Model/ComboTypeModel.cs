using RedMoonGames.Database;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Combo.Models
{
    [Serializable]
    public class ComboTypeModel : IDatabaseModelPrimaryKey<string>
    {
        public string ComboName;
        public InputActionReference[] InputActions;
        //public float BeforeAttackInteval;
        public float CanAttackInteval;
        public float AfterComboInterval;
        public string AnimationTrigger;
        public Sprite FrameSprite;
        public Sprite IconSprite;

        public string PrimaryKey => ComboName;
    }
}