using System;
using UnityEngine;
using Zenject;

public class TimeController : MonoBehaviour
{
	private bool _paused;
	
	public bool Paused => _paused;
	
	public event Action<bool> OnPause;
	
	public void Pause(bool pause)
	{
		_paused = pause;
		Time.timeScale = pause ? 0 : 1;
		OnPause?.Invoke(pause);
	}
}
