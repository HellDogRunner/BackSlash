using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
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
