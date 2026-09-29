using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent (typeof(EnemyBase))]
[RequireComponent (typeof(Movement))]
public class EnemySukima : MonoBehaviour
{
    //敵が来るまで待つ

    public enum Action
    {
        Idle,//
        Wait,//見てわかる予備動作　近づくと掴み
        Feint,//距離で急に掴みに入る
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
    //private Vector3 m_toPlayer => m_playerPos.Value - transform.position;

    //==============================
    //Flag
    //==============================

    [SerializeField] private float m_checkDiscoveryDistance;
    public float CheckDis => m_checkDiscoveryDistance;

    private Action m_previousAction = Action.Idle;

    private Coroutine m_isAction;

    //coolTime
    private float m_coolTimer;
    private bool IsCoolTime => m_coolTimer > 0f;
    

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
        m_actionCalculateList.Add(new ActionCalculate(Action.Wait));
        m_actionCalculateList.Add(new ActionCalculate(Action.Feint));
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

    }

    private void FixedUpdate()
    {
        if(m_enemyBase.State == EnemyBase.EnemyState.Attack ||
            m_enemyBase.State == EnemyBase.EnemyState.Disable ||
            IsCoolTime)
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
    private float m_evaluateFeintValue;

    private float m_globalAlert => GameManager.Instance.GlobalAlert;

    private void UpdateEvaluation()
    {
        EvaluateIdle();
        EvaluateWait();
        EvaluateFeint();
    }

    private void EvaluateIdle()
    {
        //基礎値
        m_evaluateIdleValue = 20;

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Idle);

        action.m_value = m_evaluateIdleValue;
    }

    private void EvaluateWait()
    {
        //基礎値
        m_evaluateWaitValue = 10;

        if(m_sqrDistance <= CheckDis * CheckDis * 2f)
        {
            m_evaluateWaitValue += 20;
        }

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Wait);

        action.m_value = m_evaluateWaitValue;
    }


    private void EvaluateFeint()
    {
        //基礎値
        m_evaluateFeintValue = 10;

        if (m_globalAlert >= 70f)
        {
            m_evaluateFeintValue += 20;
        }
        else if(m_globalAlert >= 40f)
        {
            m_evaluateFeintValue += 10;

        }

        if (m_sqrDistance <= CheckDis * CheckDis * 2f)
        {
            m_evaluateWaitValue += 10;
        }

        var action = m_actionCalculateList.Find(x => x != null && x.m_action == Action.Feint);

        action.m_value = m_evaluateFeintValue;
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

            case Action.Wait:
                m_isAction = StartCoroutine(ExecuteChase());
                break;

            case Action.Feint:
                m_isAction = StartCoroutine(ExecuteFeint());
                break;
        }

        m_previousAction = m_decideAction;
    }

    private void ExecuteIdle()
    {
       
    }

    private IEnumerator ExecuteChase()
    {

        //anim.idle
        float startTime = Time.time;
        float timeout = 10f;
        while (true)
        {
            if (m_sqrDistance <= CheckDis * CheckDis * 2f)
            {
                //anim.attack

                yield return new WaitForSeconds(2f);

                m_coolTimer = Random.Range(3f, 6f);

                break;
            }

            //タイムアウト時　もう一回判断
            if (Time.time - startTime >= timeout)
            {
                m_coolTimer = 0f;
                break;
            }

            yield return null;
        }


        m_isAction = null;
    }

    private IEnumerator ExecuteFeint()
    {
        //anim.idle
        float startTime = Time.time;
        float timeout = 10f;
        while (true)
        {
            if (m_sqrDistance <= CheckDis * CheckDis * 2f)
            {
                //anim.attack

                yield return new WaitForSeconds(2f);

                m_coolTimer = Random.Range(6f, 12f);

                break;
            }

            //タイムアウト時　もう一回判断
            if (Time.time - startTime >= timeout)
            {
                m_coolTimer = 0f;
                break;
            }

            yield return null;
        }

        

        m_isAction = null;
    }

}
