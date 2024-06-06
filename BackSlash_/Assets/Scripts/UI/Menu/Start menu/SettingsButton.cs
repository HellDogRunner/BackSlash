using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class SettingsButton : BaseButton
    {
        protected override void HandleButtonClick()
        {
            Debug.Log("Settings");
        }
    }
}
