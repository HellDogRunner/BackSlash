using UnityEngine;

namespace RedMoonGames.Window
{
	public class StartWindow : BasicWindow
	{	
		[SerializeField] private WindowHandler _mainHandler;

		protected override void OnEnable()
		{
			base.OnEnable();
			
			_uiInputs.OnEscapeKeyPressed -= Hide;
		}

		protected override void Showed(WindowHandler handler)
		{
			if (_thisHandler == handler)
			{
				_uiInputs.OnAnyKeyPressed += AnyKey;
			}
		}
				
		private void AnyKey()
		{
			if (!_animator.Active())
			{
				_uiInputs.OnAnyKeyPressed -= AnyKey;
				_windowService.TryOpenWindow(_mainHandler);
				_windowService.ShowWindow(_mainHandler);
				Close();
			}
		}
	}
}
