using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Database
{
	[Serializable]
	[CreateAssetMenu(fileName = "TraderScriptableDatabase", menuName = "Scriptable Objects/TraderScriptableDatabase")]
	public class ItemsScriptableDatabase<TData> : ScriptableObject
	{
		[SerializeField] public int _currency;
		[SerializeField] protected List<TData> _blades = new List<TData>();
		
		public int GetCurrency()
		{
			return _currency;
		}

		public void SetCurrency(int value)
		{ 
			_currency = value;
		}
		
		public List<TData> GetData()
		{
			return _blades;
		}
	}
}
