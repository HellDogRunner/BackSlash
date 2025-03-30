using System;
using System.Collections;
using Scripts.Entity;
using UnityEngine;

public class StabilityController : MonoBehaviour
{ 
    [SerializeField] private StabilityModel _stability;
    
    private Coroutine _recovery;

    public event Action OnStun;

    public void OnAwake(StabilityModel model)
    {
        _stability = model;
        _stability.Current = _stability.Max;
    }
    
    public void Damage(int value)
    {
        if (_stability.Current == 0) return;
        
        _stability.Current -= value;
        
        RestartCoroutine();
        
        if (_stability.Current <= 0)
        {
            _stability.Current = 0;
            RestartCoroutine(false);
            Stun();
        }
    }
    
    public void FillUp()
    {
        _stability.Current = _stability.Max;
    }
    
    private void Stun()
    {
        OnStun?.Invoke();
    }

    private void RestartCoroutine(bool needStart = true)
    {
        if (_recovery != null) StopCoroutine(_recovery);
        if (needStart) _recovery = StartCoroutine(Recovery());
    }

    private IEnumerator Recovery()
    {
        yield return new WaitForSeconds(_stability.TimeToRecovery);
        
        while (_stability.Current < _stability.Max)
        {
            yield return new WaitForSeconds(_stability.RecoveryInterval);
            _stability.Current += 1;
        }
    }
}
