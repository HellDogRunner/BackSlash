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
		
		protected readonly Dictionary<IWindow, WindowHandler> _createdWindows = new Dictionary<IWindow, WindowHandler>();

		public event Action<bool, float> OnShowWindow;
		public event Action OnUnpause;
		public event Action OnPause;

		public void Unpause()
		{
			OnUnpause?.Invoke();
		}
		
		public void Pause()
		{
			OnPause?.Invoke();
		}
		
		public void ShowWindow(bool pause = false, float delay = 0)
		{
			OnShowWindow?.Invoke(pause, delay);
		}

		public TryResult TryOpenWindow(WindowHandler window, WindowModel model = null)
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
			return TryResult.Successfully;
		}

		public void CloseWindow(WindowHandler handler)
		{
			if (_createdWindows.TrySearchKeyByValue(handler, out var window))
			{
				window.Close();
			}
		}

		private IWindow CreateWindow(CachedBehaviour prefab, WindowHandler windowHandler)
		{
			var windowGameObject = _diContainer.InstantiatePrefab(prefab, windowsRoot);

			if (!windowGameObject.TryGetComponent<IWindow>(out var window))
			{
				Destroy(windowGameObject);
				return null;
			}

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

			window.OnWindowClose -= WindowClose;

			_createdWindows.Remove(window);

			var windowCachedBehaviour = window as CachedBehaviour;
			if (windowCachedBehaviour == null)
			{
				return;
			}

			Destroy(windowCachedBehaviour.gameObject);
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
		
		public IWindow GetWindowByHandler(WindowHandler handler)
		{
			if (_createdWindows.TrySearchKeyByValue(handler, out var window))
			{
				return window;
			}
			return null;
		}
	}
}
