using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Database
{
    [Serializable]
    [CreateAssetMenu(fileName = "InteractionScriptableDatabase", menuName = "Scriptable Objects/InteractionScriptableDatabase")]
    public class QuestScriptableDatabase<TData> : ScriptableObject
    {
        [SerializeField] protected List<TData> _activeQuests = new List<TData>();

        public List<TData> GetDialogue()
        {
            return _activeQuests;
        }
    }
}