using UnityEngine;

namespace Scripts.Menu
{
	public class MenuController : MenuElement
	{
		private void Awake()
		{
			menu.view.SetCurrency();
			InstantObjects();
		}
		
		private void InstantObjects()
		{
			Instantiate(menu.model.playerProjection, transform);
			Instantiate(menu.model.weaponProjection, transform);
		}
	}
}
