using UnityEngine;
using RedMoonGames.Basics;
using System.Collections.Generic;
using Zenject;

namespace RedMoonGames.Window
{
    public class WindowService : CachedBehaviour
    {
        [SerializeField] private WindowServiceSettings settings;
        [Space]
        [SerializeField] private Transform windowsRoot;

        [Inject] private DiContainer _diContainer;

        protected readonly Dictionary<IWindow, WindowHandler> _createdWindows = new Dictionary<IWindow, WindowHandler>();

        public TryResult TryShowWindow(WindowHandler window, WindowModel model = null)
        {
            if(settings == null)
            {
                return TryResult.Fail;
            }

            if(!settings.TryGetWindowSettings(window, out var windowSettings) || windowSettings.WindowPrefab == null)
            {
                return TryResult.Fail;
            }

            if(!_createdWindows.TrySearchKeyByValue(window, out var openedWindow) || !windowSettings.IsSingle)
            {
                openedWindow = CreateWindow(windowSettings.WindowPrefab, window);
                if(openedWindow == null)
                {
                    return TryResult.Fail;
                }
            }

            openedWindow.SetModel(model);
            openedWindow.Show();
            return TryResult.Successfully;
        }

        public void CloseWindow(IWindow window)
        {
            if (!_createdWindows.ContainsKey(window))
            {
                return;
            }

            window.Close();
        }

        private IWindow CreateWindow(CachedBehaviour prefab, WindowHandler windowHandler)
        {
            var windowGameObject = _diContainer.InstantiatePrefab(prefab, windowsRoot);
            windowGameObject.SetActive(false);

            if (!windowGameObject.TryGetComponent<IWindow>(out var window))
            {
                Destroy(windowGameObject);
                return null;
            }

            window.OnWindowShow += WindowShow;
            window.OnWindowClose += WindowClose;

            _createdWindows.Add(window, windowHandler);
            return window;
        }

        private void RemoveWindow(IWindow window)
        {
            if (!_createdWindows.ContainsKey(window))
            {
                return;
            }

            window.OnWindowShow -= WindowShow;
            window.OnWindowClose -= WindowClose;

            _createdWindows.Remove(window);

            var windowCachedBehaviour = window as CachedBehaviour;
            if (windowCachedBehaviour == null)
            {
                return;
            }

            Destroy(windowCachedBehaviour.gameObject);
        }

        private void WindowShow(IWindow window)
        {
            var windowCachedBehaviour = window as CachedBehaviour;
            if(windowCachedBehaviour == null)
            {
                return;
            }

            windowCachedBehaviour.gameObject.SetActive(true);
        }

        private void WindowClose(IWindow window, WindowCloseContext closeContext)
        {
            var windowCachedBehaviour = window as CachedBehaviour;
            if (windowCachedBehaviour == null)
            {
                return;
            }

            windowCachedBehaviour.gameObject.SetActive(false);
            RemoveWindow(window);
        }

        public IWindow ReturnWindow(WindowHandler window)
        {
            if (_createdWindows.TrySearchKeyByValue(window, out var openedWindow))
            {
                return openedWindow;
            }
            return null;
        }
    }
}