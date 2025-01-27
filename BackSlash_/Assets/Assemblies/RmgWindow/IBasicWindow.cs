using System;
using RedMoonGames.Basics;

namespace RedMoonGames.Window
{
	public abstract class IBasicWindow : CachedBehaviour, IWindow
	{
		public event Action<IWindow> OnWindowShow;
		public event Action<IWindow, WindowCloseContext> OnWindowClose;

		private WindowModel _lastWindowModel;

		public virtual void Show(bool delay = false)
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

        public virtual void SetHandler(WindowHandler windowHandler)
        {
        }

		public virtual void SetModel(WindowModel windowModel)
		{
			_lastWindowModel = windowModel;
		}
    }
}
