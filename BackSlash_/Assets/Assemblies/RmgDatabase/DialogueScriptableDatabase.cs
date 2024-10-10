using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Database
{
    [Serializable]
    public class DialogueScriptableDatabase<TData, TData2> : ScriptableObject
    {
        [SerializeField] protected string NPCName;
        [SerializeField] protected List<TData> _dialogue = new List<TData>();
        [SerializeField] protected List<TData2> _answer = new List<TData2>();

        public List<TData> GetDialogue()
        {
            return _dialogue;
        }

        public string GetName()
        {
            return NPCName;
        }

        public List<TData2> GetStates()
        {
            return _answer;
        }
    }
}
