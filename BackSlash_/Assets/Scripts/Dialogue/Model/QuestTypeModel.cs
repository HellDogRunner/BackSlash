using RedMoonGames.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI.Dialogue
{
    [Serializable]
    public class QuestTypeModel : IDatabaseModelPrimaryKey<string>
    {
        public string State;
        public string[] Phrases;
        public List<QuestQuestionsModel> Questions;
        public List<QuestEndingsModel> Endings;

        public string PrimaryKey => State;
    }
}