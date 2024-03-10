using RedMoonGames.Basics;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace RedMoonGames.SceneLoader
{
    public class SceneLoader : CachedBehaviour
    {
        [SerializeField] private bool isBootstrapperEnabled = true;
        [SerializeField] private int bootstrapperSceneIndex = 0;

        private SceneLoaderService _sceneLoaderService;
        private SceneLoaderContext _loadedContext;

        public event Action<SceneLoaderContext> OnSceneLoaded;

        [Inject]
        private void Construct(SceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        private void Start()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (!isBootstrapperEnabled || _sceneLoaderService.ActiveSceneIndex == activeScene.buildIndex)
            {
                return;
            }

            if(SceneManager.sceneCountInBuildSettings < bootstrapperSceneIndex)
            {
                throw new Exception($"Scene with index {bootstrapperSceneIndex} not found");
            }

            if(activeScene.buildIndex == bootstrapperSceneIndex)
            {
                throw new Exception($"Bootstrapper has same scene index as active scene ({activeScene.buildIndex})");
            }

            var bootstrapperSceneParams = new LoadSceneParameters(LoadSceneMode.Single);
            var loadedScene = SceneManager.LoadScene(bootstrapperSceneIndex, bootstrapperSceneParams);
        }

        public TryResult TryLoad(SceneLoaderContext loaderContext)
        {
            if (!IsValidSceneLoaderContext(loaderContext))
            {
                return TryResult.Fail;
            }

#if UNITY_EDITOR
            Load(loaderContext);
            return TryResult.Successfully;
#else
            try
            {
                Load(loaderContext);
            }
            catch(Exception exception)
            {
                return TryResult.Exception(exception);
            }

            return TryResult.Successfully;
#endif
        }

        public void Load(SceneLoaderContext loaderContext)
        {
            _loadedContext = loaderContext;
            SceneContextLoaded(_loadedContext);

            OnSceneLoaded?.Invoke(loaderContext);
        }

        public virtual bool IsValidSceneLoaderContext(SceneLoaderContext loaderContext)
        {
            return true;
        }

        protected virtual void SceneContextLoaded(SceneLoaderContext loaderContext)
        {
        }
    }
}
