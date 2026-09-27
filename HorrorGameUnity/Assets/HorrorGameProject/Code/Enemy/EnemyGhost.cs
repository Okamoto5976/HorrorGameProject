using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(EnemyBase))]
[RequireComponent (typeof(Movement))]
public class EnemyGhost : MonoBehaviour
{
    //動き方に緩急をつける
    //目的　進路の妨害
    //定期的に消える

    public enum GhostAction
    {
        Idle,
        Chase,
        FastChase,
        Wandering,
        Feint,
        Block
    }

    //================================
    // Component References
    //================================

    private EnemyBase m_enemyBase;
    private Movement m_movement;



    
    [SerializeField] private float m_checkDiscoveryDistance;

    private Vector3 m_facingDir;

    private Vector3 m_moveDir;

    public class ActionCalculate
    {
        public GhostAction m_action;
        public float m_value;

        public ActionCalculate(GhostAction action)
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
    }

    private void Start()
    {
        m_actionCalculateList = new List<ActionCalculate>();
        m_actionCalculateList.Add(new ActionCalculate(GhostAction.Idle));
        m_actionCalculateList.Add(new ActionCalculate(GhostAction.Chase));
        m_actionCalculateList.Add(new ActionCalculate(GhostAction.FastChase));
        m_actionCalculateList.Add(new ActionCalculate(GhostAction.Wandering));
        m_actionCalculateList.Add(new ActionCalculate(GhostAction.Feint));
        m_actionCalculateList.Add(new ActionCalculate(GhostAction.Block));


        //初期右向き
        m_facingDir = Vector3.right;
    }

    private void Update()
    {
        UpdateEvaluation();
        DecideAction();

        if(m_debugMode.Value)
        {
            m_debugText.text = m_decideAction.ToString();
        }
    }

    private void FixedUpdate()
    {
        if(m_enemyBase.State == EnemyBase.EnemyState.Attack ||
            m_enemyBase.State == EnemyBase.EnemyState.Disable)
        {
            m_movement.Move(Vector3.zero);

        }
        else
        {
            m_movement.Move(m_moveDir);

        }

        ExecuteAction();

    }

    //==============================
    //情報取得
    //==============================

    //private void UpdatePerception()
    //{

    //}

    //==============================
    //情報をスコアに加工
    //==============================

    private float m_evaluateIdleValue;
    private float m_evaluateChaseValue;
    private float m_evaluateFastChaseValue;
    private float m_evaluateWanderingValue;
    private float m_evaluateFeintValue;
    private float m_evaluateBlockValue;

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

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == GhostAction.Idle);

        action.m_value = m_evaluateIdleValue;
    }

    private void EvaluateChase()
    {
        //基礎値
        m_evaluateChaseValue = 20;

        if(m_sqrDistance <= 10f * 10f)
        {
            m_evaluateChaseValue += 20;


            //Player発見
            if(m_toPlayer.x * m_facingDir.x > 0f)
            {
                m_evaluateChaseValue += 10;
            }
        }



        var action = m_actionCalculateList.Find(x => x != null && x.m_action == GhostAction.Chase);

        action.m_value = m_evaluateChaseValue;
    }

    private void EvaluateFastChase()
    {
        //基礎値
        m_evaluateFastChaseValue = 20;

        if (m_sqrDistance <= 10f * 10f)
        {
            m_evaluateFastChaseValue += 20;


            //Player発見かつPlayerがこちらを見ていない
            //Playerが左　かつ　ToPlayer（敵からみたPlayerの方向）が左のとき　/ Playerが右　かつ　ToPlayer（敵からみたPlayerの方向)が右のとき
            //上の条件＋ ToPlayer*facingdir > 0f
            if(m_playerFacingDir.Value.x * m_toPlayer.x > 0f)
            {
                if (m_toPlayer.x * m_facingDir.x > 0f)
                {
                    m_evaluateFastChaseValue += 40;

                    //Debug.Log("距離内かつよそ見");
                }
            }
        }

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == GhostAction.FastChase);

        action.m_value = m_evaluateFastChaseValue;
    }

    private void EvaluateWandering()
    {
        var action = m_actionCalculateList.Find(x => x != null && x.m_action == GhostAction.Wandering);

        action.m_value = m_evaluateWanderingValue;
    }

    private void EvaluateFeint()
    {
        var action = m_actionCalculateList.Find(x => x != null && x.m_action == GhostAction.Feint);

        action.m_value = m_evaluateFeintValue;
    }

    private void EvaluateBlock()
    {
        //10m以上から発見して＋10
        //警戒値が70以上で+40
        //警戒値が70ないとChaseの基礎値に届かないようにする

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == GhostAction.Block);

        action.m_value = m_evaluateBlockValue;
    }

    //==============================
    //一番適した行動を決定
    //==============================

    private float m_baseValue;
    private GhostAction m_decideAction = GhostAction.Idle;

    private void DecideAction()
    {
        m_baseValue = 0f;

        foreach(var action in m_actionCalculateList)
        {
            float value = action.m_value;

            if(value > m_baseValue)
            {
                m_baseValue = value;
                m_decideAction = action.m_action;
            }
        }
    }

    //==============================
    //決定した行動を実行
    //==============================

    private void ExecuteAction()
    {
        switch(m_decideAction)
        {
            case GhostAction.Idle:
                break;

            case GhostAction.Chase:
                break;

            case GhostAction.FastChase:
                break;

            case GhostAction.Wandering:
                break;

            case GhostAction.Feint:
                break;

            case GhostAction.Block:
                break;
        }

        //chase
        //fastChase
        //wandering
        //feint
        //block
    }
}
