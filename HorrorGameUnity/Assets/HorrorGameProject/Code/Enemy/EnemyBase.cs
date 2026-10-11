using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(ReturnPool))]
public abstract class EnemyBase : MonoBehaviour
{
    //ヒット処理
    //Playerを認識
    //ReturnPool
    [SerializeField] private LayerMask m_playerLayer;

    public enum EnemyState
    {
        Idle,
        Stay,//当たり判定なし　行動する
        Attack,
        Disable
    }

    protected EnemyState m_state = EnemyState.Idle;

    public EnemyState State => m_state;

    [SerializeField] private EnemyState m_defaultState;

    //================================
    // Component References
    //================================

    private ReturnPool m_returnPool;



    [SerializeField] private bool m_isResistEnable;


    //private bool m_isHitCollider = true;


    private void Awake()
    {
        m_returnPool = GetComponent<ReturnPool>();
    }

    private void OnEnable()
    {
        ChangeState(m_defaultState);
    }

    protected abstract void Init();

    //public void SetIsHitCollider(bool value) => m_isHitCollider = value;

    public void ResistPlayer()
    {
        ChangeState(EnemyState.Disable);

        Invoke(nameof(RecoverfromDisable), 3f);
    }

    private void RecoverfromDisable()
    {
        ChangeState(m_defaultState);
        Debug.Log("DefaultState");
    }

    protected void ChangeState(EnemyState state)
    {
        if (m_state == state) return;

        m_state = state;
    }

    //public void ReturnPool()
    //{
    //    m_returnPool.CallReturnPool();
    //}

    private void OnTriggerStay(Collider other)
    {

        //if (!m_isHitCollider) return;

        if (m_state == EnemyState.Stay) return;
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
