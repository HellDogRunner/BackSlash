using RedMoonGames.Basics;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class WindowService : CachedBehaviour
    {
        [SerializeField] private WindowServiceSettings settings;
        [Space]
        [SerializeField] public Transform windowsRoot;

        [Inject] private DiContainer _diContainer;

        private WindowHandler _activeWindow;

        protected readonly Dictionary<IWindow, WindowHandler> _createdWindows = new Dictionary<IWindow, WindowHandler>();

        public event Action<WindowHandler> OnHideWindow;
        public event Action OnShowWindow;
        public event Action OnUnpause;
        public event Action<string> OnSwitchScene;

        public void ChangeScene(string sceneName)
        {
            OnSwitchScene?.Invoke(sceneName);
        }

        public void Unpause()
        {
            OnUnpause?.Invoke();
        }

        public void ShowWindow(WindowHandler window)
        {
            OpenWindow(window);
            OnShowWindow?.Invoke();
        }

        public void HideWindow()
        {
            OnHideWindow?.Invoke(_activeWindow);
        }

        public void OpenWindow(WindowHandler window)
        {
            TryShowWindow(window);
            _activeWindow = window;
        }

        public void CloseWindow(WindowHandler window)
        {
            ReturnWindow(window)?.Close();
        }

        public TryResult TryShowWindow(WindowHandler window, WindowModel model = null)
        {
            if (settings == null)
            {
                return TryResult.Fail;
            }

            if (!settings.TryGetWindowSettings(window, out var windowSettings) || windowSettings.WindowPrefab == null)
            {
                return TryResult.Fail;
            }

            if (!_createdWindows.TrySearchKeyByValue(window, out var openedWindow) || !windowSettings.IsSingle)
            {
                openedWindow = CreateWindow(windowSettings.WindowPrefab, window);
                if (openedWindow == null)
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
            if (windowCachedBehaviour == null)
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