using UnityEngine;

namespace RedMoonGames.Window
{
    public class ManagementTab : BasicTab
    {
        private void OnEnable()
        {
            _audioManager.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);

            _selectedTabImage.enabled = true;
        }

        private void OnDisable()
        {
            _selectedTabImage.enabled = false;
        }
    }
}
