using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //=========================
    // Component References
    //==========================

    public float speed = 5f; //Test

    private Rigidbody m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        //transform.position = new Vector2(dir.x * speed, transform.position.y);

        m_rb.linearVelocity = new Vector3(dir.x * speed, m_rb.linearVelocity.y);
    }

}
