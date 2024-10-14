using RedMoonGames.Basics;
using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.UI.Dialogue
{
    [CreateAssetMenu(fileName = "Quest", menuName = "[RMG] Scriptable/Quests/QuestDatabase")]
    public class QuestDatabase : DialogueScriptableDatabase<QuestTypeModel, QuestStateModel>
    {
        public QuestTypeModel GetModelByState(string state)
        {
            if (state == "") return null;

            return _questData.GetBy(model => model.State == state);
        }

        public QuestStateModel GetAnswerByState(string state)
        {
            if (state == "") return null;

            return _stateTransitions.GetBy(model => model.State == state);
        }
    }
}