using UnityEngine;

[RequireComponent (typeof(ReturnPool))]
public class EnemyBase : MonoBehaviour
{
    //ƒqƒbƒgˆ—
    //Player‚ð”FŽ¯
    //ReturnPool
    [SerializeField] private LayerMask m_playerLayer;

    public enum EnemyState
    {
        Idle,
        Attack,
        Disable
    }

    private EnemyState m_state = EnemyState.Idle;

    public EnemyState State => m_state;

    //================================
    // Component References
    //================================

    private ReturnPool m_returnPool;

    //[SerializeField] private float m_checkDistance;

    ////get playerPos
    //[SerializeField] private Vector3Asset m_playerPos;

    [SerializeField] private bool m_isResistEnable;

    private bool m_isHitCollider = true;


    private void Awake()
    {
        m_returnPool = GetComponent<ReturnPool>();
    }

    public void SetIsHitCollider(bool value) => m_isHitCollider = value;

    public void ResistPlayer()
    {
        ChangeState(EnemyState.Disable);
    }
    //private bool CheckPlayerPos()
    //{
    //    return (Vector3.Distance(transform.position, m_playerPos.Value) <= m_checkDistance);
    //}

    //private void OnMoveDir()
    //{
    //    if(m_playerPos.Value.x < transform.position.x)
    //    {
    //        //m_moveInput.x = -1f;
    //    }
    //    else
    //    {
    //        //m_moveInput.x = 1f;
    //    }
    //}

    private void ChangeState(EnemyState state)
    {
        if (m_state == state) return;

        m_state = state;
    }

    public void ReturnPool()
    {
        m_returnPool.CallReturnPool();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!m_isHitCollider) return;

        if (m_state == EnemyState.Disable) return;
        if (m_state == EnemyState.Attack) return;

        //layer
        if ((m_playerLayer.value & (1 << other.gameObject.layer)) == 0) return;
        

        var target = other.GetComponentInParent<PlayerController>();

        if (target == null) return;



        if (m_isResistEnable)
        {
            target.HitEnemy(this);

            ChangeState(EnemyState.Attack);
        }
        else
        {
            target.KillPlayer();

            ChangeState(EnemyState.Disable);

        }


    }
}
