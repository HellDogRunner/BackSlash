using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class StartGameButton : BaseButton
    {
        protected override void HandleButtonClick()
        {
            SceneTransition.SwichToScene("FirstLocation");
        }
    }
}
