using UnityEngine;

namespace Scripts.Menu
{
	public class MenuElement : MonoBehaviour
	{
		protected PlayerMenu Menu { get { return GetComponent<PlayerMenu>(); } }
	}
}

namespace Scripts.Menu
{
	public class PlayerMenu : MonoBehaviour
	{
		public MenuModel Model;
		public MenuView View;
		public MenuController Controller;
	}
}
