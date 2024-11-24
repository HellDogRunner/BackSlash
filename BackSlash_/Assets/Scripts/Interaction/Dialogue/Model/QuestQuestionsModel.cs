using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.UI.Dialogue
{
    [Serializable]
    public class QuestQuestionsModel : IDatabaseModelPrimaryKey<int>
    {
        public int PhraseIndex;
        public string Answer1;
        public int Index1;
        public string Answer2;
        public int Index2;

        public int PrimaryKey => PhraseIndex;
    }
}