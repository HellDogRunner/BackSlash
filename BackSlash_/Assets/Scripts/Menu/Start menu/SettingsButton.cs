using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class SettingsButton : MonoBehaviour
    {
        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick()
        {
            Debug.Log("Settings button clicked!");
        }
    }
}
