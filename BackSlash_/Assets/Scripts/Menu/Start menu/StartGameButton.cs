using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class StartGameButton : MonoBehaviour
    {
        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick()
        {
            SceneTransition.SwichToScene("FirstLocation");
        }
    }
}
