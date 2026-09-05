using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    //InputAction class
    private InputSystem_Actions m_input;

    
    private Vector2 m_moveInput;
    private bool m_isRun;
    private bool m_isRest;
    private bool m_isMap;
    private bool m_isInteract;


    public void Enable()
    {
        m_input = new InputSystem_Actions();

        m_input.Player.Move.performed += InputMove;
        m_input.Player.Move.canceled += InputMove;

        m_input.Player.Sprint.performed += InputRun;
        m_input.Player.Rest.performed += InputRest;
        m_input.Player.Map.performed += InputMap;
        m_input.Player.Interact.performed += InputInteract;

        m_input.Enable();
    }

    public void Disable()
    {
        m_input.Disable();
    }

    private void InputMove(InputAction.CallbackContext context)
    {
        //Debug.Log("InputClass Call Move callback");
        m_moveInput = context.ReadValue<Vector2>();
    }

    private void InputRun(InputAction.CallbackContext context)
    {
        //Debug.Log("InputClass Call Run callback");
        m_isRun = true;
    }

    private void InputRest(InputAction.CallbackContext context)
    {
        //Debug.Log("InputClass Call Rest callback");
        m_isRest = true;
    }

    private void InputMap(InputAction.CallbackContext context)
    {
        //Debug.Log("InputClass Call Map callback");
        m_isMap = true;
    }

    private void InputInteract(InputAction.CallbackContext context)
    {
        //Debug.Log("InputClass Call Interact callback");
        m_isInteract = true;
    }

    public Vector2 MoveInput
    {
        get
        {
            return m_moveInput;
        }
    }

    public bool IsRun
    {
        get
        {
            bool result = m_isRun;
            m_isRun = false;

            return result;
        }
    }

    public bool IsRest
    {
        get
        {
            bool result = m_isRest;
            m_isRest = false;

            return result;
        }
    }

    public bool IsMap
    {
        get
        {
            bool result = m_isMap;
            m_isMap = false;

            return result;
        }
    }

    public bool IsInteract
    {
        get
        {
            bool result = m_isInteract;
            m_isInteract = false;

            return result;
        }
    }
}
