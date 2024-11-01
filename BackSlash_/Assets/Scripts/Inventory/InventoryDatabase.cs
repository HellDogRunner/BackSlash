using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.Inventory
{
	[CreateAssetMenu(fileName = "PlayerItems", menuName = "[RMG] Scriptable/PlayerItems")]
	public class InventoryDatabase : InventoryScriptableDatabase<TemporaryBuff, WeaponBladeMod, WeaponGuardMod>
	{
		public void AddItem(TemporaryBuff item)
		{
			_buffs.Add(item);
		}
		
		public void AddItem(WeaponBladeMod item)
		{
			_blades.Add(item);
		}
		
		public void AddItem(WeaponGuardMod item)
		{
			_guards.Add(item);
		}
		
		[ContextMenu("Unsold Invventory")]
		private void ResetHave()
		{
			// нужно расширение под новые позиции
			foreach (var item in _blades) item.Have = false;
			foreach (var item in _buffs) item.Have = false;
			foreach (var item in _guards) item.Have = false;
		}
		
		[ContextMenu("Reset Buffs")]
		public void ResetBuffs() 
		{
			foreach(var buff in _buffs)
			{
				buff.Damage = false;
				buff.AttackSpeed = false;
				buff.Resistance = false;
				buff.Burning = false;
				buff.Frost = false;
				buff.Poison = false;
				buff.Shock = false;
				buff.Bleed = false;
				buff.setted = false;
				buff.Value = 0;
			}
		}
		
		[ContextMenu("Clear Inventory")]
		private void ClearInventory()
		{
			_blades.Clear();
			_buffs.Clear();
			_guards.Clear();
		}
	}
}