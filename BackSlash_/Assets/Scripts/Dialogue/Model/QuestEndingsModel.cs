using RedMoonGames.Database;
using System;
namespace Scripts.UI.Dialogue
{
    [Serializable]
    public class QuestEndingsModel : IDatabaseModelPrimaryKey<int>
    {
        public int Index;

        public string State;

        public int PrimaryKey => Index;
    }
}