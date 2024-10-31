using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Database
{
	[Serializable]
	[CreateAssetMenu(fileName = "TraderScriptableDatabase", menuName = "Scriptable Objects/TraderScriptableDatabase")]
	public class InventoryScriptableDatabase<Buffs, Blades, Guards> : ScriptableObject
	{
		[SerializeField] public int _currency;
		[SerializeField] protected List<Buffs> _buffs = new List<Buffs>();
		[SerializeField] protected List<Blades> _blades = new List<Blades>();
		[SerializeField] protected List<Guards> _guards = new List<Guards>();
		
		public int GetCurrency()
		{
			return _currency;
		}

		public void SetCurrency(int value)
		{ 
			_currency = value;
		}
		
		public List<Buffs> GetBuffs()
		{
			return _buffs;
		}
		
		public List<Blades> GetBlades()
		{
			return _blades;
		}
		
		public List<Guards> GetGuards() 
		{
			return _guards;
		}
	}
}