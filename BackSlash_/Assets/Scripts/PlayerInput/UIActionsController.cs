using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
	public class UIActionsController : MonoBehaviour
	{
		private GameControls _inputActions;

		public event Action OnEnterKeyPressed;
		public event Action OnEscapeKeyPressed;
		public event Action OnSwitchTabPressed;
		public event Action OnBackKeyPressed;
		public event Action OnAnyKeyPressed;
		public event Action OnTradeKeyPressed;
		public event Action<int> OnMenuTabPressed;
		public event Action<int> OnMenuSwitchTabAction;
		public event Action<bool> ShowCursor;
		public event Action<bool> OnDialogueAnswer;

		private void Awake()
		{
			_inputActions = new GameControls();
		}

		private void Enter(InputAction.CallbackContext context)
		{
			OnEnterKeyPressed?.Invoke();
		}

		private void Escape(InputAction.CallbackContext context)
		{
			OnEscapeKeyPressed?.Invoke();
		}

		private void TabsNavigation(InputAction.CallbackContext context)
		{
			OnSwitchTabPressed?.Invoke();
		}

		private void AnyUIKey(InputAction.CallbackContext context)
		{
			ShowCursor?.Invoke(false);
		}

		private void MousePointChange(InputAction.CallbackContext context)
		{
			ShowCursor?.Invoke(true);
		}

		private void Back(InputAction.CallbackContext context)
		{
			OnBackKeyPressed?.Invoke();
		}

		private void AnyKey(InputAction.CallbackContext context)
		{
			OnAnyKeyPressed?.Invoke();
		}

		private void Trade(InputAction.CallbackContext context) 
		{
			OnTradeKeyPressed?.Invoke();
		}

		private void Weapon(InputAction.CallbackContext context)
		{
			OnMenuTabPressed?.Invoke(0);
		}

		private void Combos(InputAction.CallbackContext context)
		{
			OnMenuTabPressed?.Invoke(1);
		}

		private void Abilities(InputAction.CallbackContext context)
		{
			OnMenuTabPressed?.Invoke(2);
		}

		private void Skills(InputAction.CallbackContext context)
		{
			OnMenuTabPressed?.Invoke(3);
		}

		private void Journal(InputAction.CallbackContext context)
		{
			OnMenuTabPressed?.Invoke(4);
		}

		private void Map(InputAction.CallbackContext context)
		{
			OnMenuTabPressed?.Invoke(5);
		}

		private void Prev(InputAction.CallbackContext context)
		{
			OnMenuSwitchTabAction?.Invoke(-1);
		}

		private void Next(InputAction.CallbackContext context)
		{
			OnMenuSwitchTabAction?.Invoke(+1);
		}

		private void PositiveAnswer(InputAction.CallbackContext context)
		{
			OnDialogueAnswer?.Invoke(true);
		}

		private void NegativeAnswer(InputAction.CallbackContext context)
		{
			OnDialogueAnswer?.Invoke(false);
		}

		private void SubscribeToActions()
		{
			_inputActions.UI.Enter.performed += Enter;
			_inputActions.UI.Escape.performed += Escape;
			_inputActions.UI.Back.performed += Back;
			_inputActions.UI.SwitchTab.performed += TabsNavigation;
			_inputActions.UI.PrevTab.performed += Prev;
			_inputActions.UI.NextTab.performed += Next;

			_inputActions.UI.Point.performed += MousePointChange;
			_inputActions.UI.AnyKey.performed += AnyKey;
			_inputActions.UI.AnyKey.performed += AnyUIKey;

			_inputActions.UI.Weapon.performed += Weapon;
			_inputActions.UI.Combos.performed += Combos;
			_inputActions.UI.Abilities.performed += Abilities;
			_inputActions.UI.Skills.performed += Skills;
			_inputActions.UI.Journal.performed += Journal;
			_inputActions.UI.Map.performed += Map;

			_inputActions.UI.PositiveAnswer.performed += PositiveAnswer;
			_inputActions.UI.NegativeAnswer.performed += NegativeAnswer;
			_inputActions.UI.Trade.performed += Trade;
		}

		private void UnsubscribeToActions()
		{
			_inputActions.UI.Enter.performed -= Enter;
			_inputActions.UI.Escape.performed -= Escape;
			_inputActions.UI.Back.performed -= Back;
			_inputActions.UI.SwitchTab.performed -= TabsNavigation;
			_inputActions.UI.PrevTab.performed -= Prev;
			_inputActions.UI.NextTab.performed -= Next;

			_inputActions.UI.Point.performed -= MousePointChange;
			_inputActions.UI.AnyKey.performed -= AnyKey;
			_inputActions.UI.AnyKey.performed -= AnyUIKey;

			_inputActions.UI.Weapon.performed -= Weapon;
			_inputActions.UI.Combos.performed -= Combos;
			_inputActions.UI.Abilities.performed -= Abilities;
			_inputActions.UI.Skills.performed -= Skills;
			_inputActions.UI.Journal.performed -= Journal;
			_inputActions.UI.Map.performed -= Map;

			_inputActions.UI.PositiveAnswer.performed -= PositiveAnswer;
			_inputActions.UI.NegativeAnswer.performed -= NegativeAnswer;
			_inputActions.UI.Trade.performed -= Trade;
		}

		private void OnEnable()
		{
			_inputActions.Enable();
			SubscribeToActions();
		}

		private void OnDisable()
		{
			_inputActions.Disable();
			UnsubscribeToActions();
		}
	}
}