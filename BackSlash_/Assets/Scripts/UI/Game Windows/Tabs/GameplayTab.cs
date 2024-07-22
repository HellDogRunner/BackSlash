using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedMoonGames.Window
{
    public class GameplayTab : BasicTab
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