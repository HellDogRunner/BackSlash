using UnityEngine;

namespace RedMoonGames.Window
{
	public class StartWindow : BasicWindow
	{	
		[SerializeField] protected float _showDelay = 0.3f;
			
		private void Awake()
		{
			_animator.ShowWindow(_canvasGroup, _showDelay);
		}
	}
}
