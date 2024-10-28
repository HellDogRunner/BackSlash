using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TradeSystem : MonoBehaviour
{
	private InteractionSystem _interactionSystem;
	
	//public event Action<List<GameObject>> OnSetInventory;
	
	[Inject]
	private void Construct(InteractionSystem interactionSystem) 
	{
		_interactionSystem = interactionSystem;
	}
	
	private void Awake()
	{
		_interactionSystem.OnExitTrigger += SetDefault;
	}
	
	private void SetDefault()
	{
		
	}
	
	private void OnDestroy()
	{
		_interactionSystem.OnExitTrigger -= SetDefault;
	}
}