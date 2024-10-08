using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.UI.PlayerMenu
{
    [Serializable]
    public class TabTypeModel : IDatabaseModelPrimaryKey<string>
    {
        public string Name;
        public GameObject Prefab;
        public int Index;

        public string PrimaryKey => Name;
    }
}