using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Database
{
    [Serializable]
    public class DialogueScriptableDatabase<TData> : ScriptableObject
    {
        [SerializeField] protected List<TData> _questData = new List<TData>();

        public List<TData> QuestDialogue()
        {
            return _questData;
        }
    }
}