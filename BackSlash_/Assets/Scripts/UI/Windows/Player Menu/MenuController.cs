namespace Scripts.Menu
{
	public class MenuController : MenuElement
	{
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
			Instantiate(model.PlayerProjection, model.Root);
			Instantiate(model.WeaponProjection, model.Root);
		}
		
		private void SetWeaponMods()
		{
			var data = model.EquippedItems.GetData();
			
			view.Hilt.SetItem(data);
			view.Guard.SetItem(data);
			view.Blade.SetItem(data);
		}
	}
}