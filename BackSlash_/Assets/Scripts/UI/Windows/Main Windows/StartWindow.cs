using UnityEngine;

namespace RedMoonGames.Window
{
	public class StartWindow : GameBasicWindow
	{	
		[SerializeField] protected float _showDelay = 0.3f;
		
		
		private void OnEnable()
		{
			_animator.ShowWindow(_canvasGroup, _showDelay);
		}
	}
}
