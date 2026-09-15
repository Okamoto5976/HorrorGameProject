using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    //InputAction class
    private InputSystem_Actions m_input;


    //private Vector2 m_moveInput;
    //private bool m_isRun;
    //private bool m_isRest;
    //private bool m_isMap;
    //private bool m_isInteract;
    public event Action<Vector2> OnMoveInput;
    public event Action<bool> OnSprint;
    public event Action OnRest;
    public event Action OnMap;
    public event Action OnInteract;



    public void Enable()
    {
        m_input = new InputSystem_Actions();

        m_input.Player.Move.performed += InputMove;
        m_input.Player.Move.canceled += InputMove;

        m_input.Player.Sprint.performed += InputRun;
        m_input.Player.Sprint.canceled += InputRun;

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
        Vector2 dir = context.ReadValue<Vector2>();

        OnMoveInput?.Invoke(dir);
    }

    private void InputRun(InputAction.CallbackContext context)
    {
        bool value = false;

        if(context.performed)
        {
            //Debug.Log("InputClass Call Run callback");
            value = true;
        }
        else
        {
            //m_isRun = false;
        }
        OnSprint?.Invoke(value);

        //Debug.Log(m_isRun);
    }

    private void InputRest(InputAction.CallbackContext context)
    {
        //Debug.Log("InputClass Call Rest callback");
        //m_isRest = true;
        OnRest?.Invoke();
    }

    private void InputMap(InputAction.CallbackContext context)
    {
        //Debug.Log("InputClass Call Map callback");
        //m_isMap = true;
        OnMap?.Invoke();
    }

    private void InputInteract(InputAction.CallbackContext context)
    {
        //Debug.Log("InputClass Call Interact callback");
        //m_isInteract = true;
        OnInteract?.Invoke();
    }

    //public Vector2 MoveInput
    //{
    //    get
    //    {
    //        return m_moveInput;
    //    }
    //}

    //public bool IsRun
    //{
    //    get
    //    {
    //        return m_isRun;
    //    }
    //}

    //public bool IsRest
    //{
    //    get
    //    {
    //        bool result = m_isRest;
    //        m_isRest = false;

    //        return result;
    //    }
    //}

    //public bool IsMap
    //{
    //    get
    //    {
    //        bool result = m_isMap;
    //        m_isMap = false;

    //        return result;
    //    }
    //}

    //public bool IsInteract
    //{
    //    get
    //    {
    //        bool result = m_isInteract;
    //        m_isInteract = false;

    //        return result;
    //    }
    //}
}
