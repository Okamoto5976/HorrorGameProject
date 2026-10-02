using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(EnemyBase))]
[RequireComponent (typeof(Movement))]
public class EnemyTeke : MonoBehaviour
{
    //================================
    // Component References
    //================================

    private EnemyBase m_enemyBase;
    private Movement m_movement;
    private SpriteRenderer m_renderer;

    //================================
    //情報取得
    //================================
    //get playerPos
    [SerializeField] private Vector3Asset m_playerPos;

    [SerializeField] private Vector3Asset m_playerFacingDir;

    [SerializeField] private RuntimeFloat m_playerStamina;

    //Distance
    private float m_sqrDistance => (m_playerPos.Value - transform.position).sqrMagnitude;

    //ToPlayer
    private Vector3 m_toPlayer;

    //==============================
    //Flag
    //==============================

    [SerializeField] private float m_checkDiscoveryDistance;
    public float CheckDis => m_checkDiscoveryDistance;

    //[SerializeField] private float m_checkAttackDistance;
    //public float AttackDis => m_checkAttackDistance;

    //private Action m_previousAction = Action.Idle;

    private Coroutine m_isAction;

    [SerializeField] private float m_waitTime;

    private bool m_isEnable = false;
    private bool m_isPreparation = false;
    //coolTime
    private float m_waitTimer;

    //値からアラート分引く
    private bool IsWait => m_waitTimer > m_waitTime;

    private float m_globalAlert => GameManager.Instance.GlobalAlert;


    //playerが離れたかどうか
    //Anim かつ　準備タイムを減らす
    //準備タイムが０に　＝＞　IsAttack
    //FixedUpdate 移動


    //==============================
    //Debug
    //==============================

    [SerializeField] private RuntimeBool m_debugMode;

    [SerializeField] private TMPro.TextMeshPro m_debugText;

    //==============================
    // Unity
    //==============================

    private void Awake()
    {
        m_enemyBase = GetComponent<EnemyBase>();
        m_movement = GetComponent<Movement>();
        m_renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (m_debugMode.Value)
        {
            m_debugText.text = $"EnemyState : {m_enemyBase.State}";
        }

        if (!m_isEnable) return;

        if (IsWait) return;

        UpdateFlag();
    }

    private void UpdateFlag()
    {
        if (!m_isPreparation)
        {
            if (m_sqrDistance >= CheckDis * CheckDis)
            {
                //anim

                Debug.Log("探知");

                m_isPreparation = true;
                m_waitTimer = 0f;
            }

            return;
        }

        m_waitTimer += Time.deltaTime;

        if(IsWait)
        {
            //anim run
            Debug.Log("発進");


            m_toPlayer = m_playerPos.Value - transform.position;
            m_enemyBase.ChangeState(EnemyBase.EnemyState.Idle);
        }


    }

    private void FixedUpdate()
    {
        if(m_enemyBase.State == EnemyBase.EnemyState.Attack ||
            m_enemyBase.State == EnemyBase.EnemyState.Disable
            )
        {
            return;
        }

        if (!IsWait) return;

        ExecuteAction();

    }



    private void ExecuteAction()
    {
        m_movement.Move(m_toPlayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponentInParent<PlayerController>();

        if (target == null) return;

        Debug.Log("起動");
        m_isEnable = true;
    }

    
}
