using UnityEngine;

[RequireComponent (typeof(ReturnPool))]
public class EnemyController : MonoBehaviour
{
    public enum GhostState
    { 
        Idle,
        Move,
        Attack
    }
    
    private GhostState m_state = GhostState.Idle;

    [SerializeField] private float m_checkDistance;

    //get playerPos
    [SerializeField] private Vector3Asset m_playerPos;

    private ReturnPool m_returnPool;

    private void Awake()
    {
        m_returnPool = GetComponent<ReturnPool>();
    }

    private void FixedUpdate()
    {
        //m_velocity = m_rb.linearVelocity;
        
        if(m_state == GhostState.Move)
        {
            //OnAddForce();
            //Debug.Log("Move");

        }

        //m_rb.linearVelocity = m_velocity;
    }

    private void Update()
    {
        OnMoveDir();

        if(CheckPlayerPos())
        {
            OnChangeState(GhostState.Move);

        }
    }

    private bool CheckPlayerPos()
    {
        return (Vector3.Distance(transform.position, m_playerPos.Value) <= m_checkDistance);
    }

    private void OnMoveDir()
    {
        if(m_playerPos.Value.x < transform.position.x)
        {
            //m_moveInput.x = -1f;
        }
        else
        {
            //m_moveInput.x = 1f;
        }
    }

    private void OnChangeState(GhostState state)
    {
        if (m_state == state) return;

        m_state = state;
    }

    public void ReturnPool()
    {
        m_returnPool.CallReturnPool();
    }
}
