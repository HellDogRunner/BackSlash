using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionService : MonoBehaviour
{
    [SerializeField] private SceneTransitionAnimationService _animationController;

    private static bool _needPlayOpening = false;

    private AsyncOperation _loadingScene;

    private void Start()
    {
        if (_needPlayOpening)
        {
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            _needPlayOpening = false;

            _animationController.AnimateOpening();
        }
    }
    
    public void SwichToScene(string sceneName)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _animationController.AnimateClosing();
        _loadingScene = SceneManager.LoadSceneAsync(sceneName);
        _loadingScene.allowSceneActivation = false;
    }

    public void ChangeScene()
    {
        _needPlayOpening = true;
        _loadingScene.allowSceneActivation = true;
    }
}