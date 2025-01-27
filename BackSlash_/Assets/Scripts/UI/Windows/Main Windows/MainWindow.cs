using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RedMoonGames.Window
{
	public class MainWindow : BasicWindow
	{
		[SerializeField] protected float _selectDelay = 0.05f;
		
		[Header("Handlers")]
		[SerializeField] private WindowHandler _startHandler;
		[SerializeField] private WindowHandler _settingsHandler;

		[Header("Buttons")]
		[SerializeField] private Button _start;
		[SerializeField] private Button _settings;
		[SerializeField] private Button _exit;

		private bool _canStart = false;

		private SceneTransition _sceneTransition;

		[Inject]
		private void Construct(SceneTransition sceneTransition)
		{
			_sceneTransition = sceneTransition;
		}
		
		private void Awake()
		{
			StartCoroutine(SelectDelay());
		}
		
		protected override void OnEnable()
		{
			base.OnEnable();
			_start.Select();
			_exit.onClick.AddListener(ExitButton);
			_start.onClick.AddListener(StartButton);
			_settings.onClick.AddListener(SettingsButton);
		}
		
		protected override void OnDisable()
		{	
			base.OnDisable();
			_exit.onClick.RemoveListener(ExitButton);
			_start.onClick.RemoveListener(StartButton);
			_settings.onClick.RemoveListener(SettingsButton);
		}

		protected override void Hide()
		{
			if (!_animator.Active())
			{
				_windowService.TryOpenWindow(_startHandler);
				_windowService.ShowWindow(_startHandler);
				Close();
			}
		}
		
		private void StartButton()
		{
			if (_canStart)
			{
				_sceneTransition.gameObject.SetActive(true);
				_sceneTransition.SwichToScene("FirstLocation");
				Close();
			}
		}
		
		private void SettingsButton()
		{
			ReplaceWindow(this, _settingsHandler);
		}
		
		private void ExitButton()
		{
			Application.Quit();
		}
		
		IEnumerator SelectDelay()
		{
			yield return new WaitForSeconds(_selectDelay);
			_canStart = true;
		}
	}
}
