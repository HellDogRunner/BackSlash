using System;
using RedMoonGames.Basics;

namespace RedMoonGames.Window
{
    public abstract class BasicWindow : CachedBehaviour, IWindow
    {
        public event Action<IWindow> OnWindowShow;
        public event Action<IWindow, WindowCloseContext> OnWindowClose;

        private WindowModel _lastWindowModel;

        public virtual void Show()
        {
            OnWindowShow?.Invoke(this);
        }

        public virtual void Close()
        {
            var closeContext = new WindowCloseContext
            {
                windowModel = _lastWindowModel
            };

            OnWindowClose?.Invoke(this, closeContext);
        }

        public virtual void SetModel(WindowModel windowModel)
        {
            _lastWindowModel = windowModel;
        }
    }
}
