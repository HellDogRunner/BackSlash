using UnityEngine;
using Zenject;
using static PlayerStates;

public class TimeController : MonoBehaviour
{
	private PlayerStateMachine _playerState;
	
	[Inject] private void Construct(PlayerStateMachine playerState)
	{
		_playerState = playerState;
	}
	
	private void OnEnable()
	{
		_playerState.OnPause += Pause;
		_playerState.OnChangeState += Unpause;
	}
	
	private void OnDisable()
	{
		_playerState.OnPause -= Pause;
		_playerState.OnChangeState -= Unpause;
	}
	
	private void Pause()
	{
		Time.timeScale = 0;
	}
	
	private void Unpause(EState state)
	{
		if (state != EState.Pause)
			Time.timeScale = 1;
	}
}
