using RedMoonGames.Database;
using Scripts.UI.Dialogue;
using System;

namespace Scripts.UI.Quest
{
    [Serializable]
    public class QuestTypeModel : IDatabaseModelPrimaryKey<DialogueDatabase>
    {
        public DialogueDatabase DialogueData;
        public string State;

        public DialogueDatabase PrimaryKey => DialogueData;
    }
}