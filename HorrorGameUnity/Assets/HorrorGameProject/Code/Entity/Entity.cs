using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    //component
    protected Rigidbody m_rb;
    protected Animator m_anim;


    //Move method 
    protected Vector3 m_moveInput;
    protected Vector3 m_velocity;

    [SerializeField] private float m_speed = 5f;

    
    protected void OnMove()
    {
        //m_velocity = m_rb.linearVelocity;

        m_velocity.x = m_moveInput.x * m_speed;

        //m_rb.linearVelocity = m_velocity;
    }


}
