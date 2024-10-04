namespace RedMoonGames.Window
{
    public class StartWindow : GameBasicWindow
    {
        private void OnDestroy()
        {
            _windowService.OnHideWindow -= DisablePause;
        }
    }
}