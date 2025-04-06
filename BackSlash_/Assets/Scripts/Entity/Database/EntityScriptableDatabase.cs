using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity
{ 
    public class EntityScriptableDatabase<TData1, TData2, TData3> : ScriptableObject
    {
        [SerializeField] protected int Health;
        [SerializeField] protected TData3 Stability;
        [SerializeField] protected List<TData1> Defense = new List<TData1>(6);
        [SerializeField] protected List<TData2> Attack = new List<TData2>();
    }
}
