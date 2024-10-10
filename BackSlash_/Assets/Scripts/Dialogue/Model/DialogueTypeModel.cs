using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.UI.Dialogue
{
    [Serializable]
    public class DialogueTypeModel : IDatabaseModelPrimaryKey<string>
    {
        public string State;
        public string[] Phrases;
        public Vector3[] Indexes;
        public int PositiveEnd;
        public int NegativeEnd;

        // X - the index of the phrase, after answering which the player will see a phrase with the index Y or Z.
        // Y - the index of the phrase that the player sees when the answer is positive.
        // Z - the index of the phrase that the player sees when the answer is negative.

        public string PrimaryKey => State;
    }
}