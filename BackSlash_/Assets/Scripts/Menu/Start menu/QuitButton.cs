using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class QuitButton : MonoBehaviour
    {
        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick()
        {
            Debug.Log("Quit button clicked!");
            Application.Quit();
        }
    }
}
