using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlasherInput : MonoBehaviour
{
    private GameControls _controls;

    public Vector2 Move { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool JumpHeld { get; private set; }

    public event Action JumpPressed;
    public event Action JumpReleased;
    public event Action DodgePressed;
    public event Action SlidePressed;

    private void Awake()
    {
        _controls = new GameControls();
    }

    private void OnEnable()
    {
        _controls.Enable();

        // Move
        _controls.Gameplay.WASD.performed += OnMove;
        _controls.Gameplay.WASD.canceled += OnMove;

        // Sprint (held)
        _controls.Gameplay.Sprint.started += OnSprintStarted;
        _controls.Gameplay.Sprint.canceled += OnSprintCanceled;

        // Jump (pressed + released)
        _controls.Gameplay.Jump.started += OnJumpStarted;
        _controls.Gameplay.Jump.canceled += OnJumpCanceled;

        // Dodge (pressed)
        _controls.Gameplay.Dodge.performed += OnDodge;

        // Slide: временно используем Block (пока не добавишь отдельный action Slide)
        _controls.Gameplay.Block.performed += OnSlide;
    }

    private void OnDisable()
    {
        // Move
        _controls.Gameplay.WASD.performed -= OnMove;
        _controls.Gameplay.WASD.canceled -= OnMove;

        // Sprint
        _controls.Gameplay.Sprint.started -= OnSprintStarted;
        _controls.Gameplay.Sprint.canceled -= OnSprintCanceled;

        // Jump
        _controls.Gameplay.Jump.started -= OnJumpStarted;
        _controls.Gameplay.Jump.canceled -= OnJumpCanceled;

        // Dodge
        _controls.Gameplay.Dodge.performed -= OnDodge;

        // Slide
        _controls.Gameplay.Block.performed -= OnSlide;

        _controls.Disable();

        Move = Vector2.zero;
        SprintHeld = false;
        JumpHeld = false;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        // В InputActions для WASD должен быть Value = Vector2
        Move = ctx.ReadValue<Vector2>();
    }

    private void OnSprintStarted(InputAction.CallbackContext ctx) => SprintHeld = true;
    private void OnSprintCanceled(InputAction.CallbackContext ctx) => SprintHeld = false;

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        JumpHeld = true;
        JumpPressed?.Invoke();
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        JumpHeld = false;
        JumpReleased?.Invoke();
    }

    private void OnDodge(InputAction.CallbackContext ctx) => DodgePressed?.Invoke();

    private void OnSlide(InputAction.CallbackContext ctx) => SlidePressed?.Invoke();
}

