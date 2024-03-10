using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace RedMoonGames.Basics
{
    [Serializable]
    public class UnityStack<T> : Stack<T>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<T> values = new List<T>();

        public void OnAfterDeserialize()
        {
            Clear();

            foreach(var value in values)
            {
                Push(value);
            }
        }

        public void OnBeforeSerialize()
        {
            values.Clear();

            var contentList = this.ToList();
            contentList.Reverse();

            values.AddRange(contentList);
        }
    }
}
