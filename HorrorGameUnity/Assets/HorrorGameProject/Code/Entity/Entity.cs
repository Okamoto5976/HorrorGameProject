using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public abstract class Entity : MonoBehaviour
{
    //component
    protected Rigidbody m_rb;
    protected Animator m_anim;


    //Move method 
    protected Vector3 m_moveInput;
    protected Vector3 m_velocity;

    [SerializeField] private float m_speed = 5f;

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
    }

    protected void OnMove()
    {
        //m_velocity = m_rb.linearVelocity;

        m_velocity.x = m_moveInput.x * m_speed;

        //m_rb.linearVelocity = m_velocity;
    }

    protected void OnRun()
    {
        m_velocity.x = m_moveInput.x * m_speed * 1.5f;
    }

    protected void OnAddForce()
    {
        Debug.Log(m_moveInput);
        m_rb.AddForce(m_moveInput.x * m_speed, m_moveInput.y, m_moveInput.z, ForceMode.Acceleration);
    }
}
