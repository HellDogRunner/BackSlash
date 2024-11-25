using UnityEngine;

namespace RedMoonGames.Window
{
	public class StartWindow : BasicWindow
	{	
		[SerializeField] private WindowHandler _mainHandler;
		[SerializeField] protected float _showDelay = 0.3f;

		private void Awake()
		{
			_windowService.ShowWindow(false, _showDelay);
		}

		protected override void OnEnable()
		{
			_uiInputs.OnAnyKeyPressed += AnyKey;
		}

		protected override void OnDisable()
		{		
			_uiInputs.OnAnyKeyPressed -= AnyKey;
		}
		
		private void AnyKey()
		{
			Hide();
			_windowService.TryOpenWindow(_mainHandler);
		}
	}
}
