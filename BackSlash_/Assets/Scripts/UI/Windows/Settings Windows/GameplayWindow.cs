using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
	public class GameplayWindow : GameBasicWindow
	{
		[Header("Handlers")]
		[SerializeField] private WindowHandler _settingsHandler;

		[Header("Navigation Keys")]
		[SerializeField] private Button _back;

		private void OnEnable()
		{
			_uiInputs.OnBackKeyPressed += BackButton;
			_back.onClick.AddListener(BackButton);
		}

		private void BackButton() { ReplaceWindow(this, _settingsHandler); }

		private void OnDisable()
		{
			_uiInputs.OnBackKeyPressed -= BackButton;

			_back.onClick.RemoveListener(BackButton);
		}
	}
}
