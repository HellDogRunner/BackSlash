using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.UI.Dialogue
{
    [Serializable]
    public class QuestTypeModel : IDatabaseModelPrimaryKey<string>
    {
        public string State;
        public string[] Phrases;
        public Vector3[] Questions;
        public Vector3[] Answers;
        public Vector2 Endings;

        public string PrimaryKey => State;
    }
}