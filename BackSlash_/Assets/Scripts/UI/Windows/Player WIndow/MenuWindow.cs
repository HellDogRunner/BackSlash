using UnityEngine;
namespace Scripts.Menu
{
	public class MenuElement : MonoBehaviour
	{
		protected MenuWindow menu { get { return GetComponent<MenuWindow>(); } }
	}
}

namespace Scripts.Menu
{
	public class MenuWindow : MonoBehaviour
	{
		public MenuModel model;
		public MenuView view;
		public MenuController controller;
	}
}
