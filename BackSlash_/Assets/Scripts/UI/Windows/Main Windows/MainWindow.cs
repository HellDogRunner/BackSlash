using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class MainWindow : GameBasicWindow
	{
		[SerializeField] protected float _showDelay = 0.3f;
		
		[Header("Handlers")]
		[SerializeField] private WindowHandler _settingsHandler;

		[Header("Buttons")]
		[SerializeField] private Button _start;
		[SerializeField] private Button _settings;
		[SerializeField] private Button _exit;

		private MainMenuController _menuController;

		[Inject]
		private void Construct(MainMenuController menuController)
		{
			_menuController = menuController;
		}

		private void OnEnable()
		{
			_animator.OnShowed += _start.Select;

			_start.onClick.AddListener(StartButton);
			_settings.onClick.AddListener(SettingsButton);
			_exit.onClick.AddListener(ExitButton);
			
			_animator.ShowWindow(_canvasGroup, _showDelay);
		}
		
		private void OnDisable()
		{
			_animator.OnShowed -= _start.Select;

			_start.onClick.RemoveListener(StartButton);
			_settings.onClick.RemoveListener(SettingsButton);
			_exit.onClick.RemoveListener(ExitButton);
		}
		
		private void StartButton() { _menuController.ChangeScene("FirstLocation"); }
		private void SettingsButton() { ReplaceWindow(this, _settingsHandler); }
		private void ExitButton() { Application.Quit(); }
	}
}