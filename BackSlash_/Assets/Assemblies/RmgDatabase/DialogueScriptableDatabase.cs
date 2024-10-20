using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Database
{
    [Serializable]
    public class DialogueScriptableDatabase<TData, TData2> : ScriptableObject
    {
        [SerializeField] protected List<TData> _questData = new List<TData>();
        [SerializeField] protected List<TData2> _stateTransitions = new List<TData2>();
        [SerializeField] protected string _deafultState;

        public List<TData> QuestDialogue()
        {
            return _questData;
        }

        public List<TData2> GetStates()
        {
            return _stateTransitions;
        }

        public string GetDeafultState()
        {
            return _deafultState;
        }
    }
}
