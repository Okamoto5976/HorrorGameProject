using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(EnemyBase))]
[RequireComponent (typeof(Movement))]
public class EnemyPeta : MonoBehaviour
{
    //まずは待機　Playerがコライダー接触かつ離れたら
    //Move　動いてしばらくはIsAttackは　ナシ
    //もし振り向いたら　IsAttackオン　Playerに向かって追いかける　当たれば抵抗
    //抵抗されて解除されれば　Poolに戻る
    //先に行かせた段階で消える（余韻だけ残すため　音をある程度出したらPool）
    //座標で成功か判断

    public enum Action
    {
        Idle,//
        Move,//
        Wait,//
        Attack//
    }

    //================================
    // Component References
    //================================

    private EnemyBase m_enemyBase;
    private Movement m_movement;
    private SpriteRenderer m_renderer;


    public class ActionCalculate
    {
        public Action m_action;
        public float m_value;

        public ActionCalculate(Action action)
        {
            m_action = action;
        }
    }

    private List<ActionCalculate> m_actionCalculateList;

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
    private Vector3 m_toPlayer => m_playerPos.Value - transform.position;

    //==============================
    //Flag
    //==============================

    //[SerializeField] private float m_checkDiscoveryDistance;
    //public float CheckDis => m_checkDiscoveryDistance;

    //[SerializeField] private float m_checkAttackDistance;
    //public float AttackDis => m_checkAttackDistance;

    private Action m_previousAction = Action.Idle;

    //private Coroutine m_actionCoroutine;

    //----------Active flag-----------------------------
    private bool m_isActive;

    private bool m_isTouched;
    [SerializeField] private float m_activeTimer;
    private float m_timer;
    [SerializeField] private float m_activeRange;
    //---------------------------------------------------

    private bool m_isAttack = false;

    private bool m_canAttack;//動き出してからできるようになるまで

    private bool m_disable = false;

    private float m_canAttackTimer;
    [SerializeField] private float m_canAttackTimeValue;

    private float m_moveTimer;

    private Vector3 m_velocity;

    //coolTime
    //private float m_coolTimer;
    //private bool IsCoolTime => m_coolTimer > 0f;
    

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
        m_actionCalculateList = new List<ActionCalculate>();
        m_actionCalculateList.Add(new ActionCalculate(Action.Idle));
        m_actionCalculateList.Add(new ActionCalculate(Action.Move));
        m_actionCalculateList.Add(new ActionCalculate(Action.Wait));
        m_actionCalculateList.Add(new ActionCalculate(Action.Attack));
    }

    private void Update()
    {
        ActiveFlag();

        if (!m_isActive) return;

        UpdateFlag();

        UpdateEvaluation();
        DecideAction();

        if (m_debugMode.Value)
        {
            m_debugText.text = m_decideAction.ToString();
        }
    }

    private void UpdateFlag()
    {
        if(m_canAttackTimer < m_canAttackTimeValue)
        {
            m_canAttackTimer += Time.deltaTime;

            if (m_canAttackTimer >= m_canAttackTimeValue)
            {
                m_canAttack = true;
                
            }
        }

        m_moveTimer += Time.deltaTime;


        //canattack is ture + Petaの座標より前なとき　こっちを向いたら　攻撃isAttack True
        if (!m_disable &&
        m_canAttack &&
        (m_playerFacingDir.Value.x * m_velocity.x) < 0f)
        {
            m_isAttack = true;
        }

        if (m_isAttack) return;

        //もしPlayerより前に行ったら終了
        //あとはDisableにして　音をPlayerの座標の前にしておく
        if (m_velocity.x > 0f)
        {
            if(m_playerPos.Value.x - transform.position.x < 0f)
            {
                m_disable = true;
            }
        }
        else if(m_velocity.x < 0f)
        {
            if(m_playerPos.Value.x - transform.position.x > 0f)
            {
                m_disable = true;
            }
        }
        
    }

    private void ActiveFlag()
    {
        if (m_isActive) return;

        if (!m_isTouched) return;

        m_timer += Time.deltaTime;

        if (m_timer > m_activeTimer && m_sqrDistance > m_activeRange * m_activeRange)
        {

            m_isActive = true;

            //向きを固定
            Vector3 direction = m_toPlayer;

            direction.y = 0f;
            direction.z = 0f;

            m_velocity = direction.normalized;
        }
    }

    private void FixedUpdate()
    {
        if(m_disable ||
            m_enemyBase.State == EnemyBase.EnemyState.Attack ||
            m_enemyBase.State == EnemyBase.EnemyState.Disable
            )
        {
            m_movement.Move(Vector3.zero);
            return;
        }

        ExecuteAction();

    }

    //==============================
    //情報をスコアに加工
    //==============================

    private float m_evaluateIdleValue;
    private float m_evaluateMoveValue;
    private float m_evaluateWaitValue;
    private float m_evaluateAttackValue;

    private float m_globalAlert => GameManager.Instance.GlobalAlert;

    private void UpdateEvaluation()
    {
        EvaluateIdle();
        EvaluateMove();
        EvaluateWait();
        EvaluateAttack();
    }

    private void EvaluateIdle()
    {
        //基礎値
        m_evaluateIdleValue = 20f;

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Idle);

        action.m_value = m_evaluateIdleValue;
    }
    private void EvaluateMove()
    {
        m_evaluateMoveValue = 10f;

        if(m_canAttack)
        {
            m_evaluateMoveValue += 20f;
        }

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Move);

        action.m_value = m_evaluateMoveValue;
    }

    private void EvaluateWait()
    {
        //基礎値
        m_evaluateWaitValue = 10f;

        if(m_globalAlert > 80f)
        {
            m_evaluateWaitValue += 10f;
        }
        else if (m_globalAlert > 60f)
        {
            m_evaluateWaitValue += 20f;
        }

        if (m_moveTimer > 3 && 6 < m_moveTimer)
        {
            m_evaluateWaitValue += m_moveTimer * 1.5f;
        }
        //時間を設けて　それが５～１０なら
        // value * timeで　時間ごとに増える値

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Wait);

        action.m_value = m_evaluateWaitValue;
    }

    private void EvaluateAttack()
    {
        m_evaluateAttackValue = 10f;

        if(m_isAttack)
        {
            m_evaluateAttackValue += 60f;
        }

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Attack);

        action.m_value = m_evaluateAttackValue;
    }

    //==============================
    //一番適した行動を決定
    //==============================

    private float m_baseValue;
    private Action m_decideAction = Action.Idle;

    private void DecideAction()
    {
        //if (m_actionCoroutine != null) return;

        m_baseValue = 0f;

        //List<ActionCalculate> candidates = new();

        Action setAction = Action.Idle;

        foreach (var action in m_actionCalculateList)
        {
            

            if (action.m_value > m_baseValue)
            {
                m_baseValue = action.m_value;

                setAction = action.m_action;

            }

        }

        m_decideAction = setAction;

    }

    //==============================
    //決定した行動を実行
    //==============================

    private void ExecuteAction()
    {
        //if (m_actionCoroutine != null) return;


        switch (m_decideAction)
        {
            case Action.Idle:
                ExecuteIdle();
                break;

            case Action.Move:
                ExecuteMove();
                break;

            case Action.Wait:
                ExecuteWait();
                break;

            case Action.Attack:
                ExecuteAttack();
                break;
        }

        m_previousAction = m_decideAction;
    }

    private void ExecuteIdle()
    {
        m_enemyBase.ChangeState(EnemyBase.EnemyState.Stay);
    }

    private void ExecuteMove()
    {
        m_enemyBase.ChangeState(EnemyBase.EnemyState.Stay);

        m_movement.Move(m_velocity);
    }

    private void ExecuteWait()
    {
        m_enemyBase.ChangeState(EnemyBase.EnemyState.Stay);

    }

    private void ExecuteAttack()
    {
        m_enemyBase.ChangeState(EnemyBase.EnemyState.Idle);

        m_movement.Move(m_toPlayer.normalized);
    }

    private void OnTriggerEnter(Collider other)
    {
        //接触　接触オン
        //+ 時間経過で　Activeオン
        m_isTouched = true;
    }

}
