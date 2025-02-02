using System;
using RedMoonGames.Window;
using UnityEngine;
using Zenject;

public class GameMenuController : BasicMenuController
{
	[SerializeField] private WindowHandler _pauseHandler;
	[SerializeField] private WindowHandler _menuHandler;

	private HUDController _hudController;
	
	private bool _windowHiding;
	
	public event Action<bool> OnPaused;
	public event Action<bool> BeforePaused;
	public event Action<bool> OnShowUI;

	[Inject]
	private void Construct(HUDController hudController)
	{
		_hudController = hudController;
	}

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1;
		_hudController.gameObject.SetActive(true);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_uiInputs.OnEscapeKeyPressed += OpenPause;
		_uiInputs.OnMenuKeyPressed += OpenMenu;
		_uiInputs.ShowCursor += ShowCursor;
		_animator.OnShowing += WindowShowing;
		_animator.OnShowed += WindowShowed;
		_animator.OnHiding += WindowHiding;
		_animator.OnHided += WindowHided;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_uiInputs.OnEscapeKeyPressed -= OpenPause;
		_uiInputs.OnMenuKeyPressed -= OpenMenu;
		_uiInputs.ShowCursor -= ShowCursor;
		_animator.OnShowing -= WindowShowing;
		_animator.OnShowed -= WindowShowed;
		_animator.OnHiding -= WindowHiding;
		_animator.OnHided -= WindowHided;
	}

	private void OpenPause()
	{
		TryOpenWindow(_pauseHandler);
	}

	private void OpenMenu()
	{
		TryOpenWindow(_menuHandler);
	}

	private void TryOpenWindow(WindowHandler window)
	{
		var pauseWindow = _windowService.GetWindowByHandler(_pauseHandler);
		var menuWindow = _windowService.GetWindowByHandler(_menuHandler);
		
		if (pauseWindow == null && menuWindow == null && Time.timeScale == 1)
		{
			_windowService.TryOpenWindow(window);
			_windowService.ShowWindow(window);
		}
	}
	
	private void WindowShowing(WindowHandler handler)
	{
		if (_menuHandler == handler || _pauseHandler == handler)
		{
			BeforePaused?.Invoke(true);
		}
	}
	
	private void WindowShowed(WindowHandler handler)
	{
		OnShowUI?.Invoke(!GetNoWindows());
		SetCursor();
		if (_menuHandler == handler || _pauseHandler == handler)
		{
			SetPause(true);
		}
	}

	private void WindowHiding(WindowHandler handler)
	{
		_windowHiding = true;
		OnShowUI?.Invoke(!GetNoWindows());
		SetCursor();
		SetPause(false);
	}
	
	private void WindowHided(WindowHandler handler)
	{
		_windowHiding = false;
		BeforePaused?.Invoke(false);
	}
	
	private void SetPause(bool value)
	{
		Time.timeScale = value ? 0 : 1;
		OnPaused?.Invoke(value);
	}
	
	private void SetCursor()
	{
		Cursor.lockState = GetNoWindows() ? CursorLockMode.Locked : CursorLockMode.Confined;
	}
	
	private void ShowCursor(bool value)
	{
		Cursor.visible = value;
	}
	
	private bool GetNoWindows()
	{
		return _windowService.WindowsCount == 0 || _windowService.WindowsCount == 1 && _windowHiding;
	}
}
