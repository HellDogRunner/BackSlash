namespace RedMoonGames.Window
{
	public class StartWindow : GameBasicWindow
	{	
		private void Awake()
		{
			_windowAnimator.ShowWindowWithDelay(_canvasGroup, _showDelay);
		}
		
		private void OnDestroy()
		{
			_windowService.OnHideWindow -= DisablePause;
		}
	}
}