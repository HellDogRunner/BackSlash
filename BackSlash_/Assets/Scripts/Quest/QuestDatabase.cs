using RedMoonGames.Basics;
using RedMoonGames.Database;
using Scripts.UI.Dialogue;
using UnityEngine;

namespace Scripts.UI.Quest
{
    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "[RMG] Scriptable/Quest")]
    public class QuestDatabase : QuestScriptableDatabase<QuestTypeModel>
    {
        public QuestTypeModel GetModelByDialogueData(DialogueDatabase dialogueData)
        {
            if (dialogueData == null) return null;

            return _activeQuests.GetBy(model => model.DialogueData == dialogueData);
        }

        public void AddQuest(DialogueDatabase dialogueData, string state)
        {
            var model = _modelTemplate;

            model.DialogueData = dialogueData;
            model.State = state;

            _activeQuests.Add(model);

            _modelTemplate = null;
        }
    }
}