using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Database
{
    public class ComboScriptableDatabase<TData, TData2> : ScriptableObject
    {
        [SerializeField] protected List<TData> _sequences = new List<TData>();

        [SerializeField] protected List<TData2> _inputActionSettings = new List<TData2>();

        [SerializeField] protected float _cancelDelay;

        public List<TData> GetSequenceData()
        {
            return _sequences;
        }

        public List<TData2> GetInputSettingsData()
        {
            return _inputActionSettings;
        }

        public float GetCancelDelay()
        {
            return _cancelDelay;
        }
    }
}
