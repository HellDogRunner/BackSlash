using RedMoonGames.Database;
using UnityEngine;

namespace Scripts.Inventory
{
	[CreateAssetMenu(fileName = "PlayerItems", menuName = "[RMG] Scriptable/PlayerItems")]
	public class PlayerItemsDatabase : ItemsScriptableDatabase<WeaponBladeTypeModel>
	{

		public void AddBlade(WeaponBladeTypeModel item)
		{
			_blades.Add(item);
		}

		[ContextMenu("Reset Have")]
		private void ResetHave()
		{
			// нужно расширение под новые позиции
			foreach (var item in _blades) item.Have = false;
		}
	}
}