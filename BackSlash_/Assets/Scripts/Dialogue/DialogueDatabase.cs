using RedMoonGames.Basics;
using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.UI.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueDatabase", menuName = "[RMG] Scriptable/Dialogue")]
    public class DialogueDatabase : DialogueScriptableDatabase<DialogueTypeModel, DialogueStateModel>
    {
        public DialogueTypeModel GetModelByState(string state)
        {
            if (state == "") return null;

            return _dialogue.GetBy(dialogueModel => dialogueModel.State == state);
        }

        public DialogueStateModel GetAnswerByState(string state)
        {
            if (state == "") return null;

            return _answer.GetBy(statesModel => statesModel.State == state);
        }
    }
}