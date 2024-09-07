using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class UIMenuInputs : MonoBehaviour
    {
        private UIControls _playerControls;

        public event Action OnEnterPressed;
        public event Action OnBackPressed;
        public event Action OnEscapePressed;
        public event Action<int> OnPrevPressed;
        public event Action<int> OnNextPressed;
        public event Action<int> OnInventoryPressed;
        public event Action<int> OnCombosPressed;
        public event Action<int> OnAbilitiesPressed;
        public event Action<int> OnSkillsPressed;
        public event Action<int> OnJournalPressed;
        public event Action<int> OnMapPressed;
        public event Action<bool> OnAnyActionPressed;
        public event Action<bool> OnMousePointChange;

        private void Awake()
        {
            _playerControls = new UIControls();
        }

        private void Enter(InputAction.CallbackContext context)
        {
            OnEnterPressed?.Invoke();
        }

        private void Back(InputAction.CallbackContext context)
        {
            OnBackPressed?.Invoke();
        }

        private void Escape(InputAction.CallbackContext context)
        {
            OnEscapePressed?.Invoke();
        }

        private void Prev(InputAction.CallbackContext context)
        {
            OnPrevPressed?.Invoke(-1);
        }

        private void Next(InputAction.CallbackContext context)
        {
            OnNextPressed?.Invoke(+1);
        }

        private void Inventory(InputAction.CallbackContext context)
        {
            OnInventoryPressed?.Invoke(0);
        }

        private void Combos(InputAction.CallbackContext context)
        {
            OnCombosPressed?.Invoke(1);
        }

        private void Abilities(InputAction.CallbackContext context)
        {
            OnAbilitiesPressed?.Invoke(2);
        }

        private void Skills(InputAction.CallbackContext context)
        {
            OnSkillsPressed?.Invoke(3);
        }

        private void Journal(InputAction.CallbackContext context)
        {
            OnJournalPressed?.Invoke(4);
        }

        private void Map(InputAction.CallbackContext context)
        {
            OnMapPressed?.Invoke(5);
        }

        private void AnyAction(InputAction.CallbackContext context)
        {
            OnAnyActionPressed?.Invoke(false);
        }

        private void MousePositionChanged(InputAction.CallbackContext context)
        {
            OnMousePointChange?.Invoke(true);
        }

        private void OnEnable()
        {
            _playerControls.Enable();
            SubscribeToActions();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
            UnsubscribeToActions();
        }

        private void SubscribeToActions()
        {
            _playerControls.Common.Enter.performed += Enter;
            _playerControls.Common.Back.performed += Back;
            _playerControls.Common.Escape.performed += Escape;

            _playerControls.Common.Point.performed += MousePositionChanged;
            _playerControls.Common.Navigate.performed += AnyAction;
            _playerControls.Common.Enter.performed += AnyAction;
            _playerControls.Common.Back.performed += AnyAction;
            _playerControls.Menu.Prev.performed += AnyAction;
            _playerControls.Menu.Next.performed += AnyAction;

            _playerControls.Menu.Prev.performed += Prev;
            _playerControls.Menu.Next.performed += Next;
            _playerControls.Menu.Inventory.performed += Inventory;
            _playerControls.Menu.Combos.performed += Combos;
            _playerControls.Menu.Abilities.performed += Abilities;
            _playerControls.Menu.Skills.performed += Skills;
            _playerControls.Menu.Journal.performed += Journal;
            _playerControls.Menu.Map.performed += Map;
        }

        private void UnsubscribeToActions()
        {
            _playerControls.Common.Enter.performed -= Enter;
            _playerControls.Common.Back.performed -= Back;
            _playerControls.Common.Escape.performed -= Escape;

            _playerControls.Common.Point.performed -= MousePositionChanged;
            _playerControls.Common.Navigate.performed -= AnyAction;
            _playerControls.Common.Enter.performed -= AnyAction;
            _playerControls.Common.Back.performed -= AnyAction;
            _playerControls.Menu.Prev.performed -= AnyAction;
            _playerControls.Menu.Next.performed -= AnyAction;

            _playerControls.Menu.Prev.performed -= Prev;
            _playerControls.Menu.Next.performed -= Next;
            _playerControls.Menu.Inventory.performed -= Inventory;
            _playerControls.Menu.Combos.performed -= Combos;
            _playerControls.Menu.Abilities.performed -= Abilities;
            _playerControls.Menu.Skills.performed -= Skills;
            _playerControls.Menu.Journal.performed -= Journal;
            _playerControls.Menu.Map.performed -= Map;
        }
    }
}