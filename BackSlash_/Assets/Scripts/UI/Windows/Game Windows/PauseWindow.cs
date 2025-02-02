using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class PauseWindow : BasicWindow
	{
		[Header("Handlers")]
		[SerializeField] private WindowHandler _settingsHandler;

		[Header("Buttons")]
		[SerializeField] private Button _continue;
		[SerializeField] private Button _settings;
		[SerializeField] private Button _exit;

		private SceneTransition _sceneTransition;

		[Inject]
		private void Construct(SceneTransition sceneTransition)
		{
			_sceneTransition = sceneTransition;
		}

		private void Awake()
		{
			_continue.Select();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			
			_uiInputs.OnBackKeyPressed += Hide;

			_continue.onClick.AddListener(Hide);
			_settings.onClick.AddListener(SettingsButton);
			_exit.onClick.AddListener(ExitButton);
		}

		protected override void OnDisable()
		{ 
			base.OnDisable();
			
			_uiInputs.OnBackKeyPressed -= Hide;

			_continue.onClick.RemoveListener(Hide);
			_settings.onClick.RemoveListener(SettingsButton);
			_exit.onClick.RemoveListener(ExitButton);
		}
		
		private void SettingsButton() { ReplaceWindow(this, _settingsHandler); }

		private void ExitButton()
		{
			_sceneTransition.gameObject.SetActive(true);
			_sceneTransition.SwichToScene("StartMenu");
		}
	}
}
