// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input Stuff/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""InGameActions"",
            ""id"": ""21b2eb90-5b0f-41ba-8bbc-24f517f43368"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""89e2a790-9f12-48c4-92b1-ab1310e1a85f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a4989c69-adda-4cda-b217-d6c9758633f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""c2a9986d-0379-46bc-a547-cc2befaae264"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c19932b9-aaf9-44e6-a349-28354677f5e9"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""bb94e5e9-3c44-496a-94d8-85941c06f72c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""344b253f-4194-430a-9fe8-d6b9bd6aa2db"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""0e268cbb-82bb-41cf-a979-9b15c2302aac"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b0650288-6328-4a5d-9a5d-213a89275fbb"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""0dcd23a7-1692-4e2d-a87c-199aafd77adc"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""64b03b89-3e9a-4830-b850-de71bf0b6df9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ef6ea24d-f62d-47a8-b76a-6c6f3ecf2699"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""afae1353-dd28-4732-93a6-03a634883d06"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""af3775e9-89af-44c5-a204-a3e799f8e42f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6882cd3d-0f08-4f24-be4b-373cc712c2fc"",
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
                    ""id"": ""ff2aa496-d93e-41f9-9301-8cc8bdecd179"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bfdacdce-c019-4ec1-b349-a3797c0b17c1"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2653ffec-5a75-4eef-8c4d-ba5c365e9de0"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96f3754b-80d8-438d-82b3-2d4dccc67179"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7673e3f-b50a-45b0-a287-7eb107e08588"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // InGameActions
        m_InGameActions = asset.FindActionMap("InGameActions", throwIfNotFound: true);
        m_InGameActions_Movement = m_InGameActions.FindAction("Movement", throwIfNotFound: true);
        m_InGameActions_Jump = m_InGameActions.FindAction("Jump", throwIfNotFound: true);
        m_InGameActions_Aim = m_InGameActions.FindAction("Aim", throwIfNotFound: true);
        m_InGameActions_Zoom = m_InGameActions.FindAction("Zoom", throwIfNotFound: true);
        m_InGameActions_Dodge = m_InGameActions.FindAction("Dodge", throwIfNotFound: true);
        m_InGameActions_Shoot = m_InGameActions.FindAction("Shoot", throwIfNotFound: true);
        m_InGameActions_Look = m_InGameActions.FindAction("Look", throwIfNotFound: true);
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

    // InGameActions
    private readonly InputActionMap m_InGameActions;
    private IInGameActionsActions m_InGameActionsActionsCallbackInterface;
    private readonly InputAction m_InGameActions_Movement;
    private readonly InputAction m_InGameActions_Jump;
    private readonly InputAction m_InGameActions_Aim;
    private readonly InputAction m_InGameActions_Zoom;
    private readonly InputAction m_InGameActions_Dodge;
    private readonly InputAction m_InGameActions_Shoot;
    private readonly InputAction m_InGameActions_Look;
    public struct InGameActionsActions
    {
        private @PlayerControls m_Wrapper;
        public InGameActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_InGameActions_Movement;
        public InputAction @Jump => m_Wrapper.m_InGameActions_Jump;
        public InputAction @Aim => m_Wrapper.m_InGameActions_Aim;
        public InputAction @Zoom => m_Wrapper.m_InGameActions_Zoom;
        public InputAction @Dodge => m_Wrapper.m_InGameActions_Dodge;
        public InputAction @Shoot => m_Wrapper.m_InGameActions_Shoot;
        public InputAction @Look => m_Wrapper.m_InGameActions_Look;
        public InputActionMap Get() { return m_Wrapper.m_InGameActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGameActionsActions set) { return set.Get(); }
        public void SetCallbacks(IInGameActionsActions instance)
        {
            if (m_Wrapper.m_InGameActionsActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnJump;
                @Aim.started -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnAim;
                @Zoom.started -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnZoom;
                @Dodge.started -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnDodge;
                @Dodge.performed -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnDodge;
                @Dodge.canceled -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnDodge;
                @Shoot.started -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnShoot;
                @Look.started -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_InGameActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Dodge.started += instance.OnDodge;
                @Dodge.performed += instance.OnDodge;
                @Dodge.canceled += instance.OnDodge;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public InGameActionsActions @InGameActions => new InGameActionsActions(this);
    public interface IInGameActionsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
}
