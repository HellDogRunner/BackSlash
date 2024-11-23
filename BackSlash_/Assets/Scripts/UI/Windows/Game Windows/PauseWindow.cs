using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class PauseWindow : GameBasicWindow
	{
		[Header("Handlers")]
		[SerializeField] private WindowHandler _settingsHandler;

		[Header("Buttons")]
		[SerializeField] private Button _continue;
		[SerializeField] private Button _settings;
		[SerializeField] private Button _exit;

		private GameMenuController _menuController;

		[Inject]
		private void Construct(GameMenuController menuController)
		{
			_menuController = menuController;
		}

		private void OnEnable()
		{
			_continue.Select();

			_uiInputs.OnBackKeyPressed += Hide;

			_continue.onClick.AddListener(Hide);
			_settings.onClick.AddListener(SettingsButton);
			_exit.onClick.AddListener(ExitButton);
		}

		private void OnDisable()
		{ 
			_uiInputs.OnBackKeyPressed -= Hide;

			_continue.onClick.RemoveListener(Hide);
			_settings.onClick.RemoveListener(SettingsButton);
			_exit.onClick.RemoveListener(ExitButton);
		}
		
		private void SettingsButton() { ReplaceWindow(this, _settingsHandler); }

		private void ExitButton()
		{
			_menuController.ChangeScene("StartMenu");
		}
	}
}
