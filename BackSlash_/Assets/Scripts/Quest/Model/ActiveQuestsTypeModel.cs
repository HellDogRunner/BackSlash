using RedMoonGames.Database;
using Scripts.UI.Dialogue;
using System;

namespace Scripts.UI.Quest
{
    [Serializable]
    public class ActiveQuestsTypeModel : IDatabaseModelPrimaryKey<QuestDatabase>
    {
        public QuestDatabase QuestData;
        public string State;

        public QuestDatabase PrimaryKey => QuestData;
    }
}