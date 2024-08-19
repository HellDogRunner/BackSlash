using System.Collections.Generic;
using UnityEngine;

public class CurrentComboController : MonoBehaviour
{
    [Header("Sequences")]
    [SerializeField] private GameObject _sequence1;
    [SerializeField] private GameObject _sequence2;
    [SerializeField] private GameObject _sequence3;
    [SerializeField] private GameObject _sequence4;

    private int _cancelCounter;

    private List<HUDComboController> _combos = new List<HUDComboController>();

    private void Awake()
    {
        _combos.Add(_sequence1.GetComponent<HUDComboController>());
        //_combos.Add(_sequence2.GetComponent<HUDComboController>());
        //_combos.Add(_sequence3.GetComponent<HUDComboController>());
        //_combos.Add(_sequence4.GetComponent<HUDComboController>());

        foreach (var combo in _combos)
        {
            combo.OnComboFinished += OnComboFinished;
            combo.OnComboCanceled += CancelCounter;
        }
    }

    private void OnComboFinished()
    {
        foreach (var combo in _combos)
        {

        }
    }

    private void CancelCounter()
    {
        _cancelCounter++;

        if (_cancelCounter == _combos.Count)
        {
            foreach (var combo in _combos)
            {

            }
            _cancelCounter = 0;
        }
    }

    private void OnDestroy()
    {
        foreach (var combo in _combos)
        {
            combo.OnComboFinished -= OnComboFinished;
            combo.OnComboCanceled -= CancelCounter;
        }
    }
}
