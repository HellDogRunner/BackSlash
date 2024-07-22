using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    private static bool shouoldPlayOpeningAnimation = false;

    private AsyncOperation _loadingSceneOperation;

    private SceneTransitionAnimationController _animationController;

    private void Awake()
    {
        _animationController = gameObject.GetComponent<SceneTransitionAnimationController>();
    }

    private void Start()
    {
        if (shouoldPlayOpeningAnimation)
        {
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            shouoldPlayOpeningAnimation = false;

            _animationController.PlayOpeningAnimation();
        }
    }

    public void SwichToScene(string sceneName)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _animationController.PlayClosingAnimation();
        _loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _loadingSceneOperation.allowSceneActivation = false;
    }

    public void CheckSceneLoaded()
    {
        if (_loadingSceneOperation.progress == 0.9f)
        {
            _animationController.KillSequense();
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        shouoldPlayOpeningAnimation = true;
        _loadingSceneOperation.allowSceneActivation = true;
    }
}