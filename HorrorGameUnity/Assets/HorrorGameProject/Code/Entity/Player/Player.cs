using UnityEngine;

public class Player : Entity
{
    private void FixedUpdate()
    {
        //これ必要？　コピーして　すべての演算を終えてから写す
        m_velocity = m_rb.linearVelocity;

        OnMove();

        m_rb.linearVelocity = m_velocity;
    }
}
