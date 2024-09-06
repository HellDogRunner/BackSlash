using System.Collections.Generic;
using UnityEngine;
using static ComboSystem;
using UnityEngine.InputSystem;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "ComboDatabase", menuName = "[RMG] Scriptable/Combo/ComboDatabase", order = 1)]
public class ComboDatabase : ScriptableObject
{
    public InputActionAsset inputActionAsset;

    [Serializable]
    public class Sequence
    {
        public string name;
        public InputActionReference[] inputActions;
        public float maxInputTime;         
        public string animationTrigger;
    }

    [SerializeField] private Sequence[] sequences;

    public Sequence[] GetSequences() 
    {
        return sequences;
    }

    public IEnumerable<InputActionReference> GetAllUsedActionReferences()
    {
        List<InputActionReference> filteredInputActions = new List<InputActionReference>();

        foreach (var sequence in sequences)
        {
            foreach (var inputAction in sequence.inputActions)
            {
                filteredInputActions.Add(inputAction);
            }
        }
        var result = filteredInputActions.Distinct();
        return result;
    }
}