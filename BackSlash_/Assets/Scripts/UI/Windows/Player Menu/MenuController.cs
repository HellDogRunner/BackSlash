using UnityEngine;

namespace Scripts.Menu
{
	public class MenuController : MenuElement
	{
		private GameObject player;
		private GameObject weapon;
		
		private MenuModel model;
		private MenuView view;
		
		private void Awake()
		{
			model = Menu.Model;
			view = Menu.View;
			
			AwakeInstantiate();
			SetWeaponMods();
		}
		
		private void AwakeInstantiate()
		{
			player = Instantiate(model.PlayerProjection);
			weapon = Instantiate(model.WeaponProjection);
		}
		
		private void SetWeaponMods()
		{
			var data = model.EquippedItems.GetData();
			
			view.Hilt.SetItem(data);
			view.Guard.SetItem(data);
			view.Blade.SetItem(data);
		}
		
		private void OnDestroy()
		{
			Destroy(player);
			Destroy(weapon);
		}
	}
}
