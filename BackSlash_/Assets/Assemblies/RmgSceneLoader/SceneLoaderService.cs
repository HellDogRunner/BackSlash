using RedMoonGames.Basics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedMoonGames.SceneLoader
{
    public class SceneLoaderService : CachedBehaviour
    {
        private int _activeSceneIndex = -1;
        private SceneLoaderContext _activeSceneContext;

        public int? ActiveSceneIndex
        {
            get
            {
                if(_activeSceneIndex < 0)
                {
                    return null;
                }

                return _activeSceneIndex;
            }
        }

        public TryResult TryLoadSceneWithContext(string sceneName, object dataContext = null)
        {
            var sceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
            if (sceneIndex < 0)
            {
                return TryResult.Fail;
            }

            return TryLoadSceneWithContext(sceneIndex, dataContext);
        }

        public TryResult TryLoadSceneWithContext(int sceneIndex, object dataContext = null)
        {
            if (SceneManager.sceneCountInBuildSettings < sceneIndex)
            {
                return TryResult.Fail;
            }

            _activeSceneIndex = sceneIndex;
            _activeSceneContext = new SceneLoaderContext
            {
                SceneData = dataContext
            };

            SceneManager.sceneLoaded += HandleSceneLoaded;

            var bootstrapperSceneParams = new LoadSceneParameters(LoadSceneMode.Single);
            var loadedScene = SceneManager.LoadScene(sceneIndex, bootstrapperSceneParams);
            return TryResult.Successfully;
        }

        public TryResult TryLoadSceneWithContextAsync(string sceneName, out AsyncOperation loadSceneOperation, object dataContext = null)
        {
            var sceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
            if (sceneIndex < 0)
            {
                loadSceneOperation = null;
                return TryResult.Fail;
            }

            return TryLoadSceneWithContextAsync(sceneIndex, out loadSceneOperation, dataContext);
        }

        public TryResult TryLoadSceneWithContextAsync(int sceneIndex, out AsyncOperation loadOperation, object dataContext = null)
        {
            if (SceneManager.sceneCountInBuildSettings < sceneIndex)
            {
                loadOperation = null;
                return TryResult.Fail;
            }

            var sceneContext = new SceneLoaderContext
            {
                SceneData = dataContext
            };

            var bootstrapperSceneParams = new LoadSceneParameters(LoadSceneMode.Single);
            var loadSceneOperation = SceneManager.LoadSceneAsync(sceneIndex, bootstrapperSceneParams);
            loadSceneOperation.completed += LoadSceneOperationCompleted;

            void LoadSceneOperationCompleted(AsyncOperation operation)
            {
                loadSceneOperation.completed -= LoadSceneOperationCompleted;

                _activeSceneIndex = sceneIndex;
                _activeSceneContext = sceneContext;

                TryLoadSceneContext(SceneManager.GetActiveScene(), _activeSceneContext);
            }

            loadOperation = loadSceneOperation;
            return TryResult.Successfully;
        }

        private void HandleSceneLoaded(Scene newScene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            TryLoadSceneContext(newScene, _activeSceneContext);
        }

        private TryResult TryLoadSceneContext(Scene scene, SceneLoaderContext loaderContext)
        {
            if(!scene.IsValid() || !scene.isLoaded) 
            {
                return TryResult.Fail;
            }

            var sceneGameObjects = scene.GetRootGameObjects();
            var sceneLoaderGameObject = sceneGameObjects
                .Where(sceneGameObject => sceneGameObject.GetComponentInChildren<SceneLoader>() != null)
                .FirstOrDefault();

            if (sceneLoaderGameObject == null)
            {
                return TryResult.Fail;
            }

            var sceneLoader = sceneLoaderGameObject.GetComponentInChildren<SceneLoader>();
            return sceneLoader.TryLoad(loaderContext);
        }
    }
}
