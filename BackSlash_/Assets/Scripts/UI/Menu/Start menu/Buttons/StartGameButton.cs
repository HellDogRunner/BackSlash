namespace Scripts.UI
{
    public class StartGameButton : BaseButton
    {
        protected override void HandleButtonClick()
        {
            SceneTransition.SwichToScene("FirstLocation");
        }
    }
}
