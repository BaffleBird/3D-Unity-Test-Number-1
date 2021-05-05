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
                    ""name"": ""Action1"",
                    ""type"": ""Button"",
                    ""id"": ""a4989c69-adda-4cda-b217-d6c9758633f4"",
                    ""expectedControlType"": ""Button"",
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
                    ""action"": ""Action1"",
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
        m_InGameActions_Action1 = m_InGameActions.FindAction("Action1", throwIfNotFound: true);
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
    private readonly InputAction m_InGameActions_Action1;
    public struct InGameActionsActions
    {
        private @PlayerControls m_Wrapper;
        public InGameActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_InGameActions_Movement;
        public InputAction @Action1 => m_Wrapper.m_InGameActions_Action1;
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
                @Action1.started -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnAction1;
                @Action1.performed -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnAction1;
                @Action1.canceled -= m_Wrapper.m_InGameActionsActionsCallbackInterface.OnAction1;
            }
            m_Wrapper.m_InGameActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Action1.started += instance.OnAction1;
                @Action1.performed += instance.OnAction1;
                @Action1.canceled += instance.OnAction1;
            }
        }
    }
    public InGameActionsActions @InGameActions => new InGameActionsActions(this);
    public interface IInGameActionsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnAction1(InputAction.CallbackContext context);
    }
}