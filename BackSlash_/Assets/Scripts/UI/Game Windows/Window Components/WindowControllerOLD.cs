using System;
using UnityEngine;
using Zenject;

namespace RedMoonGames.Window
{
    public class WindowControllerOLD : MonoBehaviour
    {
        //private WindowService _windowService;
        private WindowHandler _activeWindow;

        // �������� ������ � ������� ����
        public event Action<WindowHandler> OnUnpausing;
        public event Action OnPausing;

        // ��������� ����� �� �������� ����
        public event Action OnPauseHide;

        // ������� �����
        public event Action<string> OnChangeScene;

        //[Inject]
        //private void Construct(WindowService windowService)
        //{
        //    _windowService = windowService;
        //}

        public void ChangeScene(string SceneName)
        {
            OnChangeScene?.Invoke(SceneName);
        }

        public void Unpause()
        {
            OnPauseHide?.Invoke();
        }

        public void ShowWindow(WindowHandler window)
        {
            OpenWindow(window);
            OnPausing?.Invoke();
        }

        public void HideWindow()
        {
            OnUnpausing?.Invoke(_activeWindow);
        }

        public void OpenWindow(WindowHandler window)
        {
            //_windowService.TryShowWindow(window);
            _activeWindow = window;
        }

        public void CloseWindow(WindowHandler window)
        {
            //_windowService.ReturnWindow(window)?.Close();
        }
    }
}