using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    //=========================
    // Component References
    //==========================

    public float speed = 5f; //Test
    [Range(0f,1f)] private float m_obriSubtract = 1f;

    private Rigidbody m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        //transform.position = new Vector2(dir.x * speed, transform.position.y);

        m_rb.linearVelocity = new Vector3(dir.x * speed * m_obriSubtract, m_rb.linearVelocity.y);
    }

    public void Run(Vector3 dir)
    {
        m_rb.linearVelocity = new Vector3(dir.x * speed * 1.5f * m_obriSubtract, m_rb.linearVelocity.y);

    }

    public void SetObriValue(float value)
    {
        m_obriSubtract = value;
    }

    public void ResetObriValue()
    {
        m_obriSubtract = 1f;
    }

}
