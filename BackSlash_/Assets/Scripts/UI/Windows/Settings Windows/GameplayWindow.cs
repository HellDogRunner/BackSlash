using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
	public class GameplayWindow : BasicWindow
	{
		[Header("Handlers")]
		[SerializeField] private WindowHandler _settingsHandler;

		[Header("Navigation Keys")]
		[SerializeField] private Button _back;

		protected override void OnEnable()
		{
			base.OnEnable();
			
			_uiInputs.OnBackKeyPressed += BackButton;
			_back.onClick.AddListener(BackButton);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			_uiInputs.OnBackKeyPressed -= BackButton;
			_back.onClick.RemoveListener(BackButton);
		}
		
		private void BackButton() { ReplaceWindow(this, _settingsHandler); }
	}
}
