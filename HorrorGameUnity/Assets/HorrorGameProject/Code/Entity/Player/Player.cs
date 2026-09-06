using UnityEngine;

public class Player : Entity
{
    private enum PlayerState
    {
        Idle,
        Move,
        Hide,
        Rest,
        Safe, //Jizo
        Resist,
        Dead,
    }

    private PlayerState m_playerState;

    //component or class
    private InputInteractSystem m_interactSystem;
    [SerializeField] private LayerMask m_interactLayer;

    //InputProvider related--------
    private InputProvider m_input;

    private bool m_isRunning;
    private bool m_isResting;
    private bool m_isMapping;
    private bool m_isInteracting;
    //----------------------------

    //player flag
    private bool m_canMove = true;

    private bool m_openMap = false;
    private GameObject m_map;

    //player status
    [SerializeField] private float m_stamina = 100f;
    private float m_staminaTime = 0f;

    public float Stamina { private set { m_stamina = Mathf.Clamp(value, 0f, 100f); } get => m_stamina; }

    //player runtime
    [SerializeField] private Vector3Asset m_playerPos;

    protected override void Awake()
    {
        base.Awake();

        m_input = new();
        m_interactSystem = new();
    }

    private void OnEnable()
    {
        m_input.Enable();

    }

    private void OnDisable()
    {
        m_input.Disable();
    }

    public void SetMap(GameObject map)
    {
        m_map = map;
    }

    private void FixedUpdate()
    {
        //これ必要？　コピーして　すべての演算を終えてから写す
        m_velocity = m_rb.linearVelocity;

        if (m_playerState == PlayerState.Safe || m_playerState == PlayerState.Hide)
        {
            m_velocity = Vector3.zero;
            m_rb.linearVelocity = m_velocity;
            return;
        }

        if(m_canMove)
        {
            if (!m_isRunning)
            {
                OnMove();

            }
            else
            {
                OnRun();

            }
        }
        else
        {
            m_velocity = Vector3.zero;

        }



        m_rb.linearVelocity = m_velocity;
    }

    private void Update()
    {
        m_playerPos.SetValue(transform.position);

        m_moveInput = m_input.MoveInput;
        m_isRunning = m_input.IsRun;
        m_isResting = m_input.IsRest;
        m_isMapping = m_input.IsMap;
        m_isInteracting = m_input.IsInteract;

        
        //Now Player Input Interact Process
        if (m_isInteracting)
        {
            m_interactSystem.TryInteract(transform.position, m_interactLayer, this);
        }

        if(m_playerState == PlayerState.Hide)
        {
            //stop stamina
            return;
        }

        if(m_isMapping)
        {
            //open map
            if(!m_openMap)
            {
                m_openMap = true;
                m_map.SetActive(m_openMap);
                m_canMove = false;
            }
            else
            {
                m_openMap = false;
                m_map.SetActive(m_openMap);
                m_canMove = true;
            }
        }

        //Now Player State Rest Process
        if (m_playerState == PlayerState.Safe)
        {
            //player rest in jizo area, so player cann't move
            RecoverStamina(5f);

            return;
        }

        //Now Player State Move Process
        if (m_moveInput != Vector3.zero)
        {
            ChangeState(PlayerState.Move);

            //anim "move"
            if (!m_isRunning)
            {
                ConsumptionStamina(1f);

            }
            else
            {
                ConsumptionStamina(2f);
            }

        }
        else
        {
            ChangeState(PlayerState.Idle);

            RecoverStamina(1f);
        }



        

        
    }

    private void ChangeState(PlayerState state)
    {
        if(m_playerState == state) return;

        m_playerState = state;
    }

    private void ConsumptionStamina(float multiply)
    {

        if(m_staminaTime >= 0.5f)
        {
            m_staminaTime = 0f;

            Stamina -= 1f * multiply;
        }

        m_staminaTime += Time.deltaTime;

    }

    private void RecoverStamina(float multiply)
    {
        if(m_staminaTime >= 0.5f)
        {
            m_staminaTime = 0f;

            Stamina += 1f * multiply;
        }

        m_staminaTime += Time.deltaTime;
    }

    //OutSide Reference-----------------------------------------------
    public void OnInteractSafeProcess()
    {

        //Debug.Log("SafeProcess");
        if(m_playerState != PlayerState.Safe)
        {
            ChangeState(PlayerState.Safe);

        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }

    public void OnInteractHideProcess()
    {
        if (m_playerState != PlayerState.Hide)
        {
            ChangeState(PlayerState.Hide);

        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }
    //------------------------------------------------------------------

}
