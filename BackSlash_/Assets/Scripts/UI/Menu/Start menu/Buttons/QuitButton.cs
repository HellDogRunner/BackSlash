using UnityEngine;

namespace Scripts.UI
{
    public class QuitButton : BaseButton
    {
        protected override void HandleButtonClick()
        {
            Debug.Log("Quit button clicked!");
            Application.Quit();
        }
    }
}
