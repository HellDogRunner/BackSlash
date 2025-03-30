using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Scripts.Entity
{ 
    public class EntityScriptableDatabase<TData1, TData2, TData3> : ScriptableObject
    {
        [field: SerializeField] public int Health { get; protected set; }
        [field: SerializeField] public TData3 Stability { get; protected set; }
        [field: SerializeField] public List<TData1> Defense { get; protected set; } = new List<TData1>(6);
        [field: SerializeField] public List<TData2> Attack { get; protected set; } = new List<TData2>();
        [field: SerializeField] public AnimatorController Animator { get; protected set; }
    }
}
