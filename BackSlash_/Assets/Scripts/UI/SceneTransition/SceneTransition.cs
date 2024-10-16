using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedMoonGames.Window
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private LoadingSceneAnimation _animations;
        [Space]
        [SerializeField] private bool _playOpening;

        private string _sceneName;

        private AsyncOperation _loadingScene;

        public event Action OnWindowHide; 

        private void Awake()
        {
            _animations.OnLoadingEnd += ChangeScene;
            _animations.OnOpeningEnd += CloseWindow;
            _animations.OnClosingEnd += LoadScene;
        }

        private void Start()
        {
            if (_playOpening)
            {
                _animations.AnimateOpening();
            }
        }

        private void CloseWindow()
        {
            OnWindowHide?.Invoke();
        }

        public void SwichToScene(string sceneName)
        {
            _sceneName = sceneName;
            _animations.AnimateClosing();
        }

        private void LoadScene()
        {
            _loadingScene = SceneManager.LoadSceneAsync(_sceneName);
            _loadingScene.allowSceneActivation = false;
        }

        private void ChangeScene()
        {
            _loadingScene.allowSceneActivation = true;
        }

        private void OnDestroy()
        {
            _animations.OnLoadingEnd -= ChangeScene;
            _animations.OnOpeningEnd -= CloseWindow;
            _animations.OnClosingEnd -= LoadScene;
        }
    }
}