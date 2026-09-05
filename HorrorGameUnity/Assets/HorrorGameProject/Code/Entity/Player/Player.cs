using UnityEngine;

public class Player : Entity
{
    private enum PlayerState
    {
        Idle,
        Move,
        Sit,
        Rest, //Jizo

    }

    private PlayerState m_playerState;

    //component or class
    private InteractSystem m_interactSystem;
    [SerializeField] private LayerMask m_interactLayer;

    //InputProvider related--------
    private InputProvider m_input;

    private bool m_isRunning;
    private bool m_isResting;
    private bool m_isMapping;
    private bool m_isInteracting;
    //----------------------------

    //player status
    [SerializeField] private float m_stamina = 100f;
    private float m_staminaTime = 0f;

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

    private void FixedUpdate()
    {
        //これ必要？　コピーして　すべての演算を終えてから写す
        m_velocity = m_rb.linearVelocity;


        OnMove();

        m_rb.linearVelocity = m_velocity;
    }

    private void Update()
    {
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

        if(m_isMapping)
        {
            //open map
        }

        //Now Player State Rest Process
        if (m_playerState == PlayerState.Rest)
        {
            //player rest in jizo area, so player cann't move

            return;
        }

        //Now Player State Move Process
        if (m_moveInput != Vector3.zero)
        {
            ChangeState(PlayerState.Move);

            //anim "move"
            ConsumptionStamina(1f);

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

        if(m_staminaTime >= 1f)
        {
            m_staminaTime = 0f;

            m_stamina -= 1f * multiply;
        }

        m_staminaTime += Time.deltaTime;

    }

    private void RecoverStamina(float multiply)
    {
        if(m_staminaTime >= 1f)
        {
            m_staminaTime = 0f;

            m_stamina += 1f * multiply;
        }

        m_staminaTime += Time.deltaTime;
    }
}
