using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player base values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    
    [Header("Movement setting")]
    public bool analogMovement;
    
    [Header("Mouse Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;
    
    private bool m_IgnoreInput;
    
    
    private static bool m_FocusActionsSetUp;
    
    
    // Events
    public event Action OnInteractPerformed;
    
    private void Start()
    {
        if (!m_FocusActionsSetUp)
        {
            #if UNITY_EDITOR
            
            var ignoreInput = new InputAction(binding: "/Keyboard/escape");
            ignoreInput.performed += context => m_IgnoreInput = true;
            ignoreInput.Enable();
            
            var enableInput = new InputAction(binding: "/Mouse/leftButton");
            enableInput.performed += context => m_IgnoreInput = false;
            enableInput.Enable();
            
            #endif
            
        }
    }

    private void OnDestroy()
    {
        m_FocusActionsSetUp = false;
    }
    
#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        if (m_IgnoreInput)
        {
            MoveInput(Vector2.zero);
            return;
        }
        
        MoveInput(value.Get<Vector2>());
    }
    
    public void OnLook(InputValue value)
    {
        if (m_IgnoreInput)
        {
            LookInput(Vector2.zero);
            return;
        }
        
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }
    
    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }
    
    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }
    
    public void OnInteract(InputValue value)
    {
        if (m_IgnoreInput)
        {
            return;
        }

        if (value.isPressed)
        {
            foreach (System.Delegate subscriber in OnInteractPerformed.GetInvocationList())
            {
                Debug.Log("Current subscriber: " + subscriber.Target.GetType().Name + " - "  + subscriber.Method.Name);
            }
            OnInteractPerformed?.Invoke();
        }
    }
#endif
    
    
    public void MoveInput(Vector2 newLookDirection)
    {
        move = newLookDirection;
    }
    
    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }
    
    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }
    
    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
        m_IgnoreInput = !hasFocus;
    }
    
    
    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
