using RedMoonGames.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Inventory
{
    [Serializable]
    public class ModsTypeModel : IDatabaseModelPrimaryKey<string>
    {
        public string Name;

        public string PrimaryKey => Name;
    }
}