using System;

namespace RedMoonGames.Window
{
	public class WindowModel
	{
		
	}
	
	public class WindowCloseContext
	{
		public WindowModel windowModel;
	}
	
	public interface IWindow
	{
		event Action<IWindow> OnWindowShow;
		event Action<IWindow, WindowCloseContext> OnWindowClose;

		void Show(bool delay = false);
		void Close();
		void SetHandler(WindowHandler handler);
		void SetModel(WindowModel windowModel);
	}
}
