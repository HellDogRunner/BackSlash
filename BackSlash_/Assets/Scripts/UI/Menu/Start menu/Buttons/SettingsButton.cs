using UnityEngine;

namespace Scripts.UI
{
    public class SettingsButton : BaseButton
    {
        protected override void HandleButtonClick()
        {
            Debug.Log("Settings");
        }
    }
}
