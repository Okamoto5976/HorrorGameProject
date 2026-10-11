using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Movement))]
public class EnemyGhost : EnemyBase
{
    //動き方に緩急をつける
    //目的　進路の妨害
    //定期的に消える

    public enum Action
    {
        Idle,
        Spirit,//固有
        Chase,
        FastChase,
        Wandering,
        Feint,
        Block
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

    //Distance
    private float m_sqrDistance => (m_playerPos.Value - transform.position).sqrMagnitude;

    //ToPlayer
    private Vector3 m_toPlayer => m_playerPos.Value - transform.position;

    //==============================
    //Flag
    //==============================

    private float m_spiritTimer;
    public bool IsSpirit => m_spiritTimer > 0;

    
    [SerializeField] private float m_checkDiscoveryDistance;
    public float CheckDis => m_checkDiscoveryDistance;

    private Vector3 m_facingDir;

    private Vector3 m_moveDir;

    private Action m_previousAction = Action.Idle;

    private Coroutine m_isAction;

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
        m_actionCalculateList.Add(new ActionCalculate(Action.Chase));
        m_actionCalculateList.Add(new ActionCalculate(Action.FastChase));
        m_actionCalculateList.Add(new ActionCalculate(Action.Wandering));
        m_actionCalculateList.Add(new ActionCalculate(Action.Feint));
        m_actionCalculateList.Add(new ActionCalculate(Action.Block));


        //初期右向き
        m_facingDir = Vector3.right;
    }

    protected override void Init()
    {
        
    }


    private void Update()
    {
        UpdateEvaluation();
        DecideAction();

        UpdateFlag();

        if(m_facingDir.x > 0f)
        {
            m_renderer.flipX = false;

        }
        else
        {
            m_renderer.flipX = true;
        }

        if (m_debugMode.Value)
        {
            m_debugText.text = m_decideAction.ToString();
        }
    }

    private void UpdateFlag()
    {
        //if(m_decideAction == Action.Idle &&
        //    m_enemyBase.State != EnemyBase.EnemyState.Disable)
        //{
        //    //Idleなとき減らす
        //    m_spiritTimer -= Time.deltaTime;
        //}
        if(m_decideAction != Action.Idle)
        {
            //Idleではないとき　spiritTimeを0に近づける
            if(m_spiritTimer < 0f)
            {
                m_spiritTimer += Time.deltaTime;
            }
        }
        else
        {

            m_spiritTimer -= Time.deltaTime;

        }

    }

    private void FixedUpdate()
    {
        if(m_state == EnemyState.Attack ||
            m_state == EnemyState.Disable)
        {
            m_movement.Move(Vector3.zero);
            return;
        }

        if (IsSpirit) return;


        ExecuteAction();

    }

    //==============================
    //情報をスコアに加工
    //==============================

    private float m_evaluateIdleValue;
    private float m_evaluateChaseValue;
    private float m_evaluateFastChaseValue;
    private float m_evaluateWanderingValue;
    private float m_evaluateFeintValue;
    private float m_evaluateBlockValue;

    private float m_alertValue => GameManager.Instance.GlobalAlert;

    private void UpdateEvaluation()
    {
        EvaluateIdle();
        EvaluateChase();
        EvaluateFastChase();
        EvaluateWandering();
        EvaluateFeint();
        EvaluateBlock();
    }

    private void EvaluateIdle()
    {
        //基礎値
        m_evaluateIdleValue = 20;

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Idle);

        action.m_value = m_evaluateIdleValue;
    }

    private void EvaluateChase()
    {
        //基礎値
        m_evaluateChaseValue = 20;

        if(m_sqrDistance <= CheckDis * CheckDis)
        {
            m_evaluateChaseValue += 20;
        }

        //Player発見
        if(m_toPlayer.x * m_facingDir.x > 0f)
        {
            m_evaluateChaseValue += 10;
        }


        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Chase);

        action.m_value = m_evaluateChaseValue;
    }

    private void EvaluateFastChase()
    {
        //基礎値
        m_evaluateFastChaseValue = 10;

        //直前がIdleじゃなければ低い
        if(m_previousAction != Action.Idle)
        {
            m_evaluateFastChaseValue -= 40;
        }

        if(m_alertValue < 30f)
        {
            m_evaluateFastChaseValue -= 40;
        }

        if (m_sqrDistance <= CheckDis * CheckDis * 1.5f)
        {
            m_evaluateFastChaseValue += 20;


        }

        //Player発見かつPlayerがこちらを見ていない
        //Playerが左　かつ　ToPlayer（敵からみたPlayerの方向）が左のとき　/ Playerが右　かつ　ToPlayer（敵からみたPlayerの方向)が右のとき
        //上の条件＋ ToPlayer*facingdir > 0f
        if (m_toPlayer.x * m_facingDir.x > 0f)
        {
            if(m_playerFacingDir.Value.x * m_toPlayer.x > 0f)
            {
                m_evaluateFastChaseValue += 30;

                //Debug.Log("距離内かつよそ見");

            }
        }

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.FastChase);

        action.m_value = m_evaluateFastChaseValue;
    }

    private void EvaluateWandering()
    {
        //基礎値
        m_evaluateWanderingValue = 50;

        //直前がIdleなら低い
        if(m_previousAction == Action.Idle)
        {
            m_evaluateWanderingValue -= 30f;
        }
        

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Wandering);

        action.m_value = m_evaluateWanderingValue;
    }

    private void EvaluateFeint()
    {
        //基礎値
        m_evaluateFeintValue = 0;

        if (m_alertValue >= 70f)
        {
            m_evaluateFeintValue += 40;
        }

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Feint);

        action.m_value = m_evaluateFeintValue;
    }

    private void EvaluateBlock()
    {
        //10m以上から発見して＋10
        //警戒値が70以上で+40
        //警戒値が70ないとChaseの基礎値に届かないようにする

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Block);

        action.m_value = m_evaluateBlockValue;
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
                ExecuteIdle();
                break;

            case Action.Chase:
                ExecuteChase();
                break;

            case Action.FastChase:
                m_isAction = StartCoroutine(ExecuteFastChase());
                break;

            case Action.Wandering:
                break;

            case Action.Feint:
                break;

            case Action.Block:
                break;
        }

        m_previousAction = m_decideAction;
        //chase
        //fastChase
        //wandering
        //feint
        //block
    }

    private void ExecuteIdle()
    {
        if(m_spiritTimer < -20f)
        {
            m_spiritTimer = Random.Range(3f, 12f);
            ChangeState(EnemyState.Disable);
            Invoke(nameof(StateIdle), m_spiritTimer);
            return;
        }

        m_movement.Move(Vector3.zero);
    }

    private void StateIdle()
    {
        ChangeState(EnemyState.Idle);
    }

    private void ExecuteChase()
    {
        m_moveDir = m_toPlayer;
        m_facingDir = m_toPlayer;

        m_movement.Move(m_moveDir.normalized);
    }

    private IEnumerator ExecuteFastChase()
    {
        m_moveDir = m_toPlayer;
        m_facingDir = m_toPlayer;



        float startTime = Time.time;
        float timeout = 3f;

        while (true)
        {
            // 行動処理
            m_movement.Run(m_moveDir.normalized);


            if (Time.time - startTime >= timeout)
            {
                break;
            }

            yield return null;
        }

        m_isAction = null;
    }

}
