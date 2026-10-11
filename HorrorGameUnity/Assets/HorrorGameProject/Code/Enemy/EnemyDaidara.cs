using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Movement))]
public class EnemyDaidara : EnemyBase
{
    //敵が来るまで待つ

    public enum Action
    {
        Idle,//
        Wait,//こちらを見ている
        Attack,//こちらに来る
    }

    //================================
    // Component References
    //================================

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

    [SerializeField] private RuntimeBool m_isMove;

    //Distance
    private float m_sqrDistance => (m_playerPos.Value - transform.position).sqrMagnitude;

    //ToPlayer
    //private Vector3 m_toPlayer => m_playerPos.Value - transform.position;

    //==============================
    //Flag
    //==============================

    [SerializeField] private float m_turnMaxValue;
    [SerializeField] private float m_backMinValue;

    //[SerializeField] private float m_checkDiscoveryDistance;
    //public float CheckDis => m_checkDiscoveryDistance;

    //[SerializeField] private float m_checkAttackDistance;
    //public float AttackDis => m_checkAttackDistance;

    private Action m_previousAction = Action.Idle;

    private Coroutine m_isAction;

    private float m_playerPreviousXPos;

    private bool IsTurn = false;


    [SerializeField] private float m_attackWaitTime;

    private float m_isAttackTimer;

    public bool IsAttack => m_isAttackTimer > m_attackWaitTime;
    

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
        m_movement = GetComponent<Movement>();
        m_renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        m_actionCalculateList = new List<ActionCalculate>();
        m_actionCalculateList.Add(new ActionCalculate(Action.Idle));
        m_actionCalculateList.Add(new ActionCalculate(Action.Wait));
        m_actionCalculateList.Add(new ActionCalculate(Action.Attack));
    }

    protected override void Init()
    {
        
    }

    private void Update()
    {
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
        if (IsAttack) return;

        if (!IsTurn)
        {
            m_isAttackTimer -= Time.deltaTime * 0.1f;
            m_isAttackTimer = Mathf.Max(m_isAttackTimer, 0f);

            return;
        }

        //Playerが動いてるか確認
        //もしうごいてるならAttackTimerを増やす　違うとき　ゆっくり下げる
        if (m_isMove.Value)
        {
            m_isAttackTimer += Time.deltaTime;
        }
        else
        {
            m_isAttackTimer -= Time.deltaTime * 0.1f;
            m_isAttackTimer = Mathf.Max(m_isAttackTimer, 0f);
        }
    }

    private void FixedUpdate()
    {
        if(m_state == EnemyState.Attack ||
            m_state == EnemyState.Disable
            )
        {
            return;
        }

        ExecuteAction();

    }

    //==============================
    //情報をスコアに加工
    //==============================

    private float m_evaluateIdleValue;
    private float m_evaluateWaitValue;
    private float m_evaluateAttackValue;

    private float m_globalAlert => GameManager.Instance.GlobalAlert;

    private void UpdateEvaluation()
    {
        EvaluateIdle();
        EvaluateWait();
        EvaluateAttack();
    }

    private void EvaluateIdle()
    {
        //基礎値
        m_evaluateIdleValue = 0;

        if(!IsTurn)
        {
            m_evaluateIdleValue += 20;
        }

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Idle);

        action.m_value = m_evaluateIdleValue;
    }

    private void EvaluateWait()
    {
        //基礎値
        m_evaluateWaitValue = 0;

        if (IsTurn)
        {
            m_evaluateWaitValue += 20;
        }

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Wait);

        action.m_value = m_evaluateWaitValue;
    }


    private void EvaluateAttack()
    {
        //基礎値
        m_evaluateAttackValue = 0;

        if (IsAttack)
        {
            m_evaluateAttackValue += 30;
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
        if (m_isAction != null) return;

        m_baseValue = 0f;

        List<ActionCalculate> candidates = new();

        foreach (var action in m_actionCalculateList)
        {
            if (action.m_value > m_baseValue)
            {
                m_baseValue = action.m_value;
                candidates.Clear();
                candidates.Add(action);
            }
            else if (action.m_value == m_baseValue)
            {
                candidates.Add(action);
            }
        }

        if (candidates.Count > 0)
        {
            int index = Random.Range(0, candidates.Count);
            m_decideAction = candidates[index].m_action;
        }
    }

    //==============================
    //決定した行動を実行
    //==============================

    private void ExecuteAction()
    {
        if (m_isAction != null) return;


        switch (m_decideAction)
        {
            case Action.Idle:
                m_isAction = StartCoroutine(ExecuteIdle());

                break;

            case Action.Wait:
                m_isAction = StartCoroutine(ExecuteWait());
                break;

            case Action.Attack:
                ExecuteAttack();
                break;
        }

        m_previousAction = m_decideAction;
    }

    private IEnumerator ExecuteIdle()
    {
        //グローバルアラートによってもっと不規則に
        //最初は15~18;  後半は3~18;
        float time = Random.Range(m_turnMaxValue - 3f, m_turnMaxValue);

        float startTime = Time.time;

        while (true)
        {
            if (Time.time - startTime >= time)
            {
                //振り向く
                break;
            }

            yield return null;

        }

        IsTurn = true;

        m_isAction = null;
    }

    private IEnumerator ExecuteWait()
    {

        //グローバルアラートによってもっと不規則に
        //最初は3f ~ 5f;  後半は3f ~15f;
        float time = Random.Range(m_backMinValue, m_backMinValue + 3f);

        float startTime = Time.time;

        while (true)
        {
            if (Time.time - startTime >= time)
            {
                //戻る
                break;
            }

            yield return null;

        }

        IsTurn = false;

        m_isAction = null;

    }

    private void ExecuteAttack()
    {
        Debug.Log("Daidara襲う");
    }

}
