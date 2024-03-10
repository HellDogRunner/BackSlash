using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Database
{
    [Serializable]
    public class ScriptableDatabase<TData> : ScriptableObject
    {
        [SerializeField] protected List<TData> _data = new List<TData>();

        public List<TData> GetData()
        {
            return _data;
        }
    }
}
