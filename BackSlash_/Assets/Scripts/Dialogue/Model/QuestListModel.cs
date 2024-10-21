using RedMoonGames.Database;
using System;
using System.Collections.Generic;

namespace Scripts.UI.Dialogue
{
    [Serializable]
    public class QuestListModel : IDatabaseModelPrimaryKey<List<int>>
    {
        public List<int> List = new List<int>(5);

        public List<int> PrimaryKey => List;
    }
}