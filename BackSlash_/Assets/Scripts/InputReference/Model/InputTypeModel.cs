using RedMoonGames.Database;
using System;

namespace Scripts.InputReference.Models
{
    [Serializable]
    public class InputTypeModel : IDatabaseModelPrimaryKey<string>
    {
        public string InputName;
        public float Length;
        public float BeforeAttackTime;
        public float CanAttackTime;

        public string PrimaryKey => InputName;
    }
}
