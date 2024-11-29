using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
	public class PlayerWindow : BasicWindow
	{
		[SerializeField] private Button _backButton;

		protected override void OnEnable()
		{
			base.OnEnable();
			
			_uiInputs.OnBackKeyPressed += Hide;
			_backButton.onClick.AddListener(Hide);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			_uiInputs.OnBackKeyPressed -= Hide;
			_backButton.onClick.RemoveListener(Hide);	
		}
	}
}
