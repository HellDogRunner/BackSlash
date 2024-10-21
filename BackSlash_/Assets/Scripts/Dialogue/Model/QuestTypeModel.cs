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
        public List<QuestListModel> Questions;
        public List<QuestListModel> Answers;
        public Vector2 Endings;

        public string PrimaryKey => State;
    }
}