using RedMoonGames.Window;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneTransitionService : MonoBehaviour
{
    [SerializeField] private SceneTransitionAnimationService _animationController;
    [Space]
    [SerializeField] private bool _isMainMenuScene;

    private static bool _needPlayOpening = false;

    private AsyncOperation _loadingScene;

    private WindowService _windowService;

    public event Action OnLoading;

    [Inject]
    private void Construct(WindowService windowService)
    {
        _windowService = windowService;
        _windowService.OnChangeScene += SwichToScene;
    }

    private void Start()
    {
        if (_needPlayOpening)
        {
            PrepareToOpenScene(); 

            _needPlayOpening = false;
            _animationController.AnimateOpening();
        }
    }
    
    public void SwichToScene(string sceneName)
    {
        OnLoading?.Invoke();
        PrepareToCloseScene();
        _animationController.AnimateClosing(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        _loadingScene = SceneManager.LoadSceneAsync(sceneName);
        _loadingScene.allowSceneActivation = false;
    }

    public void ChangeScene()
    {
        _needPlayOpening = true;
        _loadingScene.allowSceneActivation = true;
    }

    private void PrepareToCloseScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 0;
    }

    private void PrepareToOpenScene()
    {
        if (_isMainMenuScene)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        Time.timeScale = 1;   
    }
}