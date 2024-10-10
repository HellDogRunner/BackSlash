using RedMoonGames.Database;
using System;

namespace Scripts.UI.Dialogue
{
    [Serializable]
    public class DialogueStateModel : IDatabaseModelPrimaryKey<string>
    {
        public string State;
        public string Positive;
        public string Negative;

        public string PrimaryKey => State;
    }
}