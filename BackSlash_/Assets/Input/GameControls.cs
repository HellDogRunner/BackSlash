//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/GameControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""2775bd04-8890-4dfc-8b67-c5d3c80fd7b2"",
            ""actions"": [
                {
                    ""name"": ""WASD"",
                    ""type"": ""Value"",
                    ""id"": ""b45ec9ac-511e-4c78-8f5f-c5b0f6a4a6f0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Button"",
                    ""id"": ""75335dca-0260-4155-91ea-d0f072a4e3c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Value"",
                    ""id"": ""648f3bfc-c011-4d8b-941f-01b855caa686"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""65638328-091b-4310-9c8f-0a6723a4bbd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Value"",
                    ""id"": ""dbf76c3d-7947-41ff-b4ee-fb8501198a83"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ShowWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""66351d25-1992-4538-a908-dc7d821867a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HideWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""dfceaf67-c5b0-4083-8094-8c0d6780bdd9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""bca16aa9-8df2-4b94-9f0a-3b2c428313b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Value"",
                    ""id"": ""383a9525-7348-4f22-81f5-cb72d5b34aba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""TargetLock"",
                    ""type"": ""Button"",
                    ""id"": ""f7bd86bb-c072-4606-8950-9daad62bf9bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""83b3514a-173c-4e9e-9beb-d0298bdda362"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6d3597af-56e2-4719-b73b-942ae5fb393b"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""3D Vector"",
                    ""id"": ""367c6c66-1885-47e3-b0f2-37aa0c6c5e43"",
                    ""path"": ""3DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7d8ad8f2-609f-4b26-8f85-a6582485214e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ac24fa98-4db4-41b4-8a91-3650eff44808"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""65fb41b1-5170-4af0-adf9-6f6bd5c61dd5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8073fe80-0309-45b8-8d56-abaee07aedfc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fcaf5830-c5bb-4704-83cb-8450a35f93e0"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Hold(duration=9999,pressPoint=0.2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef1bb1d3-d081-4504-97a7-de542eb9c581"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31ed970c-7418-477e-81bb-ced7ccb751e6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b73de4f-9d54-4a46-a38d-a81a5596456b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShowWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1fa1f6bf-37e2-4419-b82b-ba4a16d60a0e"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HideWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""561633d1-f326-4ee0-9a9d-96dce95c9bb4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b844c81-1f4b-4ba9-8e5a-bf7c4052ec84"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold(duration=9999,pressPoint=0.2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a47b138-5eb3-46f9-afa0-d75cfe606908"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TargetLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e268fac-975f-4dd6-9fa3-295a61a6ba29"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3889aeeb-8154-4fec-a4bb-1441dfb67614"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""1dbbc10f-c1b4-4a71-bb3f-2ec79b116d0a"",
            ""actions"": [
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""209df03f-3050-425c-bb00-1e0bc315e1ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cac9fe09-b448-4176-ab6f-d8a70b849fc7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""0a85bacd-b282-4f5f-817e-350c562db0c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""eecee5d4-3a76-4c05-b21c-42ef294f8894"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""32d99416-e9d6-47aa-b68b-10f525ffd1fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0324b305-c55b-40a4-a94e-ff9cb86255d7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4687c76e-3e95-4d68-9d41-321b7b81670b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""1f2a1142-ddbf-4dbe-be6b-da8b7d3b1705"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d2047494-7fe9-472e-b69a-011387404f96"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9a9783c-721c-4e50-a6db-6e0a9628c9d7"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""6b1a2ee1-aef4-40c2-8f10-284fddc42b9b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e5e46e38-2f94-457a-b58a-56074049a926"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""065fd070-6dc6-40a0-940f-e5a4962e34a3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e080cc0f-73a3-466e-a231-f363aaa55a41"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""832e0a84-1be4-4615-8d85-880857248a46"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b5ef7fed-6cf9-454a-ac2c-dd1b1f3c26ee"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bb1a0878-27ce-4364-9a1e-6ed308d43ec2"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""158cef31-8289-471d-a2f2-88f703cfb02f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f53001a7-c92e-4b13-afb2-2957952b5b87"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""37c53744-9a54-47f4-b5cb-3dd801252d3b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c56a575-5324-4c8a-aef0-31f59d357bca"",
                    ""path"": ""<Mouse>/backButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37252def-140f-4ad3-88ff-c1cd462d52d4"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6906eb66-fd0d-4566-91f7-9a63bff6747f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93c94543-993e-40be-bb29-bdad81cf1a16"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17cf7734-f4d3-4f3f-a131-5894eabe461d"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34fd838b-5a26-4c07-aac1-3a7fbf18d3ba"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_WASD = m_Gameplay.FindAction("WASD", throwIfNotFound: true);
        m_Gameplay_Mouse = m_Gameplay.FindAction("Mouse", throwIfNotFound: true);
        m_Gameplay_Sprint = m_Gameplay.FindAction("Sprint", throwIfNotFound: true);
        m_Gameplay_Attack = m_Gameplay.FindAction("Attack", throwIfNotFound: true);
        m_Gameplay_Dodge = m_Gameplay.FindAction("Dodge", throwIfNotFound: true);
        m_Gameplay_ShowWeapon = m_Gameplay.FindAction("ShowWeapon", throwIfNotFound: true);
        m_Gameplay_HideWeapon = m_Gameplay.FindAction("HideWeapon", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Block = m_Gameplay.FindAction("Block", throwIfNotFound: true);
        m_Gameplay_TargetLock = m_Gameplay.FindAction("TargetLock", throwIfNotFound: true);
        m_Gameplay_Menu = m_Gameplay.FindAction("Menu", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Enter = m_UI.FindAction("Enter", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        m_UI_Escape = m_UI.FindAction("Escape", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
        m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
        m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
        m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_Gameplay_WASD;
    private readonly InputAction m_Gameplay_Mouse;
    private readonly InputAction m_Gameplay_Sprint;
    private readonly InputAction m_Gameplay_Attack;
    private readonly InputAction m_Gameplay_Dodge;
    private readonly InputAction m_Gameplay_ShowWeapon;
    private readonly InputAction m_Gameplay_HideWeapon;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Block;
    private readonly InputAction m_Gameplay_TargetLock;
    private readonly InputAction m_Gameplay_Menu;
    public struct GameplayActions
    {
        private @GameControls m_Wrapper;
        public GameplayActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @WASD => m_Wrapper.m_Gameplay_WASD;
        public InputAction @Mouse => m_Wrapper.m_Gameplay_Mouse;
        public InputAction @Sprint => m_Wrapper.m_Gameplay_Sprint;
        public InputAction @Attack => m_Wrapper.m_Gameplay_Attack;
        public InputAction @Dodge => m_Wrapper.m_Gameplay_Dodge;
        public InputAction @ShowWeapon => m_Wrapper.m_Gameplay_ShowWeapon;
        public InputAction @HideWeapon => m_Wrapper.m_Gameplay_HideWeapon;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Block => m_Wrapper.m_Gameplay_Block;
        public InputAction @TargetLock => m_Wrapper.m_Gameplay_TargetLock;
        public InputAction @Menu => m_Wrapper.m_Gameplay_Menu;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @WASD.started += instance.OnWASD;
            @WASD.performed += instance.OnWASD;
            @WASD.canceled += instance.OnWASD;
            @Mouse.started += instance.OnMouse;
            @Mouse.performed += instance.OnMouse;
            @Mouse.canceled += instance.OnMouse;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Dodge.started += instance.OnDodge;
            @Dodge.performed += instance.OnDodge;
            @Dodge.canceled += instance.OnDodge;
            @ShowWeapon.started += instance.OnShowWeapon;
            @ShowWeapon.performed += instance.OnShowWeapon;
            @ShowWeapon.canceled += instance.OnShowWeapon;
            @HideWeapon.started += instance.OnHideWeapon;
            @HideWeapon.performed += instance.OnHideWeapon;
            @HideWeapon.canceled += instance.OnHideWeapon;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Block.started += instance.OnBlock;
            @Block.performed += instance.OnBlock;
            @Block.canceled += instance.OnBlock;
            @TargetLock.started += instance.OnTargetLock;
            @TargetLock.performed += instance.OnTargetLock;
            @TargetLock.canceled += instance.OnTargetLock;
            @Menu.started += instance.OnMenu;
            @Menu.performed += instance.OnMenu;
            @Menu.canceled += instance.OnMenu;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @WASD.started -= instance.OnWASD;
            @WASD.performed -= instance.OnWASD;
            @WASD.canceled -= instance.OnWASD;
            @Mouse.started -= instance.OnMouse;
            @Mouse.performed -= instance.OnMouse;
            @Mouse.canceled -= instance.OnMouse;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Dodge.started -= instance.OnDodge;
            @Dodge.performed -= instance.OnDodge;
            @Dodge.canceled -= instance.OnDodge;
            @ShowWeapon.started -= instance.OnShowWeapon;
            @ShowWeapon.performed -= instance.OnShowWeapon;
            @ShowWeapon.canceled -= instance.OnShowWeapon;
            @HideWeapon.started -= instance.OnHideWeapon;
            @HideWeapon.performed -= instance.OnHideWeapon;
            @HideWeapon.canceled -= instance.OnHideWeapon;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Block.started -= instance.OnBlock;
            @Block.performed -= instance.OnBlock;
            @Block.canceled -= instance.OnBlock;
            @TargetLock.started -= instance.OnTargetLock;
            @TargetLock.performed -= instance.OnTargetLock;
            @TargetLock.canceled -= instance.OnTargetLock;
            @Menu.started -= instance.OnMenu;
            @Menu.performed -= instance.OnMenu;
            @Menu.canceled -= instance.OnMenu;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_Enter;
    private readonly InputAction m_UI_Navigate;
    private readonly InputAction m_UI_Escape;
    private readonly InputAction m_UI_Point;
    private readonly InputAction m_UI_Click;
    private readonly InputAction m_UI_ScrollWheel;
    private readonly InputAction m_UI_MiddleClick;
    private readonly InputAction m_UI_RightClick;
    public struct UIActions
    {
        private @GameControls m_Wrapper;
        public UIActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Enter => m_Wrapper.m_UI_Enter;
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputAction @Escape => m_Wrapper.m_UI_Escape;
        public InputAction @Point => m_Wrapper.m_UI_Point;
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
        public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
        public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            @Enter.started += instance.OnEnter;
            @Enter.performed += instance.OnEnter;
            @Enter.canceled += instance.OnEnter;
            @Navigate.started += instance.OnNavigate;
            @Navigate.performed += instance.OnNavigate;
            @Navigate.canceled += instance.OnNavigate;
            @Escape.started += instance.OnEscape;
            @Escape.performed += instance.OnEscape;
            @Escape.canceled += instance.OnEscape;
            @Point.started += instance.OnPoint;
            @Point.performed += instance.OnPoint;
            @Point.canceled += instance.OnPoint;
            @Click.started += instance.OnClick;
            @Click.performed += instance.OnClick;
            @Click.canceled += instance.OnClick;
            @ScrollWheel.started += instance.OnScrollWheel;
            @ScrollWheel.performed += instance.OnScrollWheel;
            @ScrollWheel.canceled += instance.OnScrollWheel;
            @MiddleClick.started += instance.OnMiddleClick;
            @MiddleClick.performed += instance.OnMiddleClick;
            @MiddleClick.canceled += instance.OnMiddleClick;
            @RightClick.started += instance.OnRightClick;
            @RightClick.performed += instance.OnRightClick;
            @RightClick.canceled += instance.OnRightClick;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            @Enter.started -= instance.OnEnter;
            @Enter.performed -= instance.OnEnter;
            @Enter.canceled -= instance.OnEnter;
            @Navigate.started -= instance.OnNavigate;
            @Navigate.performed -= instance.OnNavigate;
            @Navigate.canceled -= instance.OnNavigate;
            @Escape.started -= instance.OnEscape;
            @Escape.performed -= instance.OnEscape;
            @Escape.canceled -= instance.OnEscape;
            @Point.started -= instance.OnPoint;
            @Point.performed -= instance.OnPoint;
            @Point.canceled -= instance.OnPoint;
            @Click.started -= instance.OnClick;
            @Click.performed -= instance.OnClick;
            @Click.canceled -= instance.OnClick;
            @ScrollWheel.started -= instance.OnScrollWheel;
            @ScrollWheel.performed -= instance.OnScrollWheel;
            @ScrollWheel.canceled -= instance.OnScrollWheel;
            @MiddleClick.started -= instance.OnMiddleClick;
            @MiddleClick.performed -= instance.OnMiddleClick;
            @MiddleClick.canceled -= instance.OnMiddleClick;
            @RightClick.started -= instance.OnRightClick;
            @RightClick.performed -= instance.OnRightClick;
            @RightClick.canceled -= instance.OnRightClick;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IGameplayActions
    {
        void OnWASD(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnShowWeapon(InputAction.CallbackContext context);
        void OnHideWeapon(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnTargetLock(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnEnter(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
    }
}
