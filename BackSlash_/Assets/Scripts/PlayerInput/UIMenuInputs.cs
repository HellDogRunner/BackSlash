using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class UIMenuInputs : MonoBehaviour
    {
        private UIControls _menuControls;

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
        public event Action<bool> OnHideCursor;

        private void Awake()
        {
            _menuControls = new UIControls();
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
            OnHideCursor?.Invoke(false);
        }

        private void MousePositionChanged(InputAction.CallbackContext context)
        {
            OnHideCursor?.Invoke(true);
        }

        private void OnEnable()
        {
            _menuControls.Enable();
            SubscribeToActions();
        }

        private void OnDisable()
        {
            _menuControls.Disable();
            UnsubscribeToActions();
        }

        private void SubscribeToActions()
        {
            _menuControls.Common.Enter.performed += Enter;
            _menuControls.Common.Back.performed += Back;
            _menuControls.Common.Escape.performed += Escape;

            _menuControls.Common.Point.performed += MousePositionChanged;
            _menuControls.Common.Navigate.performed += AnyAction;
            _menuControls.Common.Enter.performed += AnyAction;
            _menuControls.Common.Back.performed += AnyAction;
            _menuControls.Menu.Prev.performed += AnyAction;
            _menuControls.Menu.Next.performed += AnyAction;

            _menuControls.Menu.Prev.performed += Prev;
            _menuControls.Menu.Next.performed += Next;
            _menuControls.Menu.Inventory.performed += Inventory;
            _menuControls.Menu.Combos.performed += Combos;
            _menuControls.Menu.Abilities.performed += Abilities;
            _menuControls.Menu.Skills.performed += Skills;
            _menuControls.Menu.Journal.performed += Journal;
            _menuControls.Menu.Map.performed += Map;
        }

        private void UnsubscribeToActions()
        {
            _menuControls.Common.Enter.performed -= Enter;
            _menuControls.Common.Back.performed -= Back;
            _menuControls.Common.Escape.performed -= Escape;

            _menuControls.Common.Point.performed -= MousePositionChanged;
            _menuControls.Common.Navigate.performed -= AnyAction;
            _menuControls.Common.Enter.performed -= AnyAction;
            _menuControls.Common.Back.performed -= AnyAction;
            _menuControls.Menu.Prev.performed -= AnyAction;
            _menuControls.Menu.Next.performed -= AnyAction;

            _menuControls.Menu.Prev.performed -= Prev;
            _menuControls.Menu.Next.performed -= Next;
            _menuControls.Menu.Inventory.performed -= Inventory;
            _menuControls.Menu.Combos.performed -= Combos;
            _menuControls.Menu.Abilities.performed -= Abilities;
            _menuControls.Menu.Skills.performed -= Skills;
            _menuControls.Menu.Journal.performed -= Journal;
            _menuControls.Menu.Map.performed -= Map;
        }
    }
}