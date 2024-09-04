using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class MenuActions : MonoBehaviour
    {
        private PlayerMenuControls _playerControls;

        public event Action OnEnterPressed;
        public event Action OnEscapePressed;
        public event Action OnBackPressed;
        public event Action OnNextPressed;
        public event Action<bool> OnAnyActionPressed;
        public event Action<bool> OnMousePointChange;
        public event Action<int> OnInventoryPressed;
        public event Action<int> OnCombosPressed;
        public event Action<int> OnAbilitiesPressed;
        public event Action<int> OnSkillsPressed;
        public event Action<int> OnJournalPressed;
        public event Action<int> OnMapPressed;

        private void Awake()
        {
            _playerControls = new PlayerMenuControls();
        }

        private void Enter(InputAction.CallbackContext context)
        {
            OnEnterPressed?.Invoke();
        }

        private void Escape(InputAction.CallbackContext context)
        {
            OnEscapePressed?.Invoke();
        }

        private void Back(InputAction.CallbackContext context)
        {
            OnBackPressed?.Invoke();
        }

        private void Next(InputAction.CallbackContext context)
        {
            OnNextPressed?.Invoke();
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
            _playerControls.PlayerMenu.Enter.performed += Enter;
            _playerControls.PlayerMenu.Escape.performed += Escape;
            _playerControls.PlayerMenu.Back.performed += Back;
            _playerControls.PlayerMenu.Next.performed += Next;

            _playerControls.PlayerMenu.Inventory.performed += Inventory;
            _playerControls.PlayerMenu.Combos.performed += Combos;
            _playerControls.PlayerMenu.Abilities.performed += Abilities;
            _playerControls.PlayerMenu.Skills.performed += Skills;
            _playerControls.PlayerMenu.Journal.performed += Journal;
            _playerControls.PlayerMenu.Map.performed += Map;

            _playerControls.PlayerMenu.Navigate.performed += AnyAction;
            _playerControls.PlayerMenu.Enter.performed += AnyAction;
            _playerControls.PlayerMenu.Back.performed += AnyAction;
            _playerControls.PlayerMenu.Next.performed += AnyAction;
            _playerControls.PlayerMenu.Point.performed += MousePositionChanged;
        }

        private void UnsubscribeToActions()
        {
            _playerControls.PlayerMenu.Enter.performed -= Enter;
            _playerControls.PlayerMenu.Escape.performed -= Escape;
            _playerControls.PlayerMenu.Back.performed -= Back;
            _playerControls.PlayerMenu.Next.performed -= Next;

            _playerControls.PlayerMenu.Inventory.performed -= Inventory;
            _playerControls.PlayerMenu.Combos.performed -= Combos;
            _playerControls.PlayerMenu.Abilities.performed -= Abilities;
            _playerControls.PlayerMenu.Skills.performed -= Skills;
            _playerControls.PlayerMenu.Journal.performed -= Journal;
            _playerControls.PlayerMenu.Map.performed -= Map;

            _playerControls.PlayerMenu.Navigate.performed -= AnyAction;
            _playerControls.PlayerMenu.Enter.performed -= AnyAction;
            _playerControls.PlayerMenu.Back.performed -= AnyAction;
            _playerControls.PlayerMenu.Next.performed -= AnyAction;
            _playerControls.PlayerMenu.Point.performed -= MousePositionChanged;
        }
    }
}