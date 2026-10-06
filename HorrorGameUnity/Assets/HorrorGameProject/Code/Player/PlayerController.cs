using UnityEngine;

[RequireComponent (typeof(Movement))]
[RequireComponent(typeof(PlayerInteract))]
[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(PlayerObari))]

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Hide,
        Rest,
        Safe, //Jizo
        Resist,
        Dead,
    }

  


    //Stateが普通の時にも　Interactができない時があるから
    private enum InteractState
    {
        Can,
        Not
    }

    public PlayerState State { get; private set; } = PlayerState.Idle;


    //================================
    // Component References
    //================================

    private Movement m_playerMovement;

    private PlayerInteract m_playerInteract;

    private PlayerStatus m_playerStatus;

    private PlayerObari m_playerObri;

    [SerializeField] private PlayerCanvas m_playerCanvas;

    //InputProvider related--------
    private InputProvider m_input;

    //private bool m_isRunning;
    //private bool m_isResting;
    //private bool m_isMapping;
    //private bool m_isInteracting;
    //----------------------------

    //player flag
    //StateがIdleの時も　動けないようにするためのフラグ
    private bool m_canMove = true;


    //variable
    private Vector2 m_inputDir;

    private bool m_isRun;

    //Right = true, Left = false;
    private bool m_resistDir;
    private EnemyBase m_resistingEnemy;
   


    //player runtime
    [SerializeField] private Vector3Asset m_playerPos;

    [SerializeField] private Vector3Asset m_playerFacingDir;

    [SerializeField] private RuntimeBool m_isMove;

    //================================
    // Unity Methods
    //================================

    private void Awake()
    {
        m_playerMovement = GetComponent<Movement>();
        m_playerInteract = GetComponent<PlayerInteract>();
        m_playerStatus = GetComponent<PlayerStatus>();
        m_playerObri = GetComponent<PlayerObari>();
        
        m_input = new();

        m_input.OnMoveInput += OnInputMove;
        m_input.OnSprint += OnInputSprint;
        m_input.OnRest += OnInputRest;
        m_input.OnMap += OnInputMap;
        m_input.OnInteract += OnInputInteract;
    }

    private void OnEnable()
    {
        m_input.Enable();

    }

    private void OnDisable()
    {
        m_input.Disable();
    }

    private void OnDestroy()
    {
        m_input.OnMoveInput -= OnInputMove;
        m_input.OnSprint -= OnInputSprint;
        m_input.OnRest -= OnInputRest;
        m_input.OnMap -= OnInputMap;
        m_input.OnInteract -= OnInputInteract;
    }

    private void Update()
    {
        m_playerPos.SetValue(transform.position);




        //Now Player State Rest Process
        if (State == PlayerState.Safe)
        {
            //player rest in jizo area, so player cann't move
            //RecoverStamina(5f);

            return;
        }

        
    }

    private void FixedUpdate()
    {
        //---Move Control-------------
        MoveControl();

    }

    private void MoveControl()
    {
        if(State == PlayerState.Safe ||
            State == PlayerState.Rest ||
            State == PlayerState.Hide ||
            State == PlayerState.Resist ||
            State == PlayerState.Dead ||
            !m_canMove)
        {
            m_playerMovement.Move(Vector3.zero);
            m_isMove.SetValue(false);

            return;
        }

        if(m_inputDir != Vector2.zero)
        {
            if(m_inputDir.x > 0f)
            {
                m_playerFacingDir.SetValue(Vector3.right);
            }
            else
            {
                m_playerFacingDir.SetValue(Vector3.left);

            }
        }

        if (m_inputDir == Vector2.zero)
        {
            m_playerMovement.Move(Vector3.zero);
            m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Idle);

            m_isMove.SetValue(false);
        }
        else if (m_isRun)
        {
            if (m_playerStatus.Stamina <= 0f) return;

            m_playerMovement.Run(m_inputDir);
            m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Run);

            m_isMove.SetValue(true);

        }
        else
        {
            m_playerMovement.Move(m_inputDir);
            m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Move);

            m_isMove.SetValue(true);

        }
    }

    private void Resist(Vector2 dir)
    {


        if (!m_resistDir)
        {
            if(dir.x > 0)
            {
                if (!m_playerStatus.ResistStamina(2f)) return;

                m_playerStatus.AddResistValue(2f);
                m_resistDir = true;
            }
        }
        else
        {
            if(dir.x < 0)
            {
                if (!m_playerStatus.ResistStamina(2f)) return;

                m_playerStatus.AddResistValue(2f);
                m_resistDir = false;
            }
        }
    }

    //================================
    // Input Event Methods
    //================================

    //Event
    private void OnInputMove(Vector2 dir)
    {
        //State is Dead => return;

        //State is Resist => A/D連打
        if(State == PlayerState.Resist)
        {
            Resist(dir);
            return;
        }

        //State is Safe => return;


        m_inputDir = dir;
    }

    private void OnInputSprint(bool value)
    {
        m_isRun = value;
    }

    private void OnInputRest()
    {
        if(State != PlayerState.Rest)
        {
            ChangeState(PlayerState.Rest);
            m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Rest);
        }
        else
        {
            ChangeState(PlayerState.Idle);
            m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Idle);
        }

    }

    private void OnInputMap()
    {
        //State is Dead => return;
        //State is 

        if(m_playerCanvas.IsActiveMapCanvas())
        {
            SetCanMove(false);
            Debug.Log(":PlayerCanvas Map is Active True");
        }
        else
        {
            SetCanMove(true);

        }
    }

    private void OnInputInteract()
    {
        m_playerInteract.TryInteract(this);

    }


    //================================
    // Public Methods
    //================================

    public void SuccessResist()
    {
        m_resistingEnemy.ResistPlayer();

        ChangeState(PlayerState.Idle);
        m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Idle);

    }



    public void HitEnemy(EnemyBase component)
    {
        m_resistingEnemy = component;

        m_playerStatus.ResetResistValue();

        ChangeState(PlayerState.Resist);
        m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Resist);

    }

    public void KillPlayer()
    {
        ChangeState(PlayerState.Dead);
    }

    public void SurpriseMap()
    {
        OnInputMap();
    }


    //================================
    // Change State Methods
    //================================

    private void ChangeState(PlayerState state)
    {
        if(State == state) return;

        State = state;
    }

    

    private void SetCanMove(bool isActive) => m_canMove = isActive;

   
     



    //OutSide Reference-----------------------------------------------
    public void ProcessSafe()
    {

        //Debug.Log("SafeProcess");
        if(State != PlayerState.Safe)
        {
            ChangeState(PlayerState.Safe);
            m_playerObri.Release();
            m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Safe);

        }
        else
        {
            ChangeState(PlayerState.Idle);
            m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Idle);
        }
    }

    public void ProcessHide()
    {
        if (State != PlayerState.Hide)
        {
            ChangeState(PlayerState.Hide);
            m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Hide);

        }
        else
        {
            ChangeState(PlayerState.Idle);
            m_playerStatus.ChangeStaminaState(PlayerStatus.StaminaState.Idle);

        }
    }

    public void SetObri()
    {
        m_playerObri.ActivationObri();
    }
    //------------------------------------------------------------------


    //外部から参照しやすいように継承化
    //しかしその理由ならInterfaceでもいい

    //動きはPlayerとEnemyとで違うことがおおい
    //細かくコンポーネント分けがいい？
    //Player
    //--PlayerController
    //--PlayerMovement
    //--PlayerAttack

    //Enemy
    //--EnemyController
    //--EnemyMovement
    //--EnemyAttack

    //共通で
    //HP
    //EntityState



    //EntityState　や　PlayerState　使ってる？　どう使う？
    
    //規模に応じてなら最初ならFindを使ってもいい？

    //コメントって日本語？企業はどうやってる？

    //Input入力　ControllerがInputのプロパティをみて判断するか　InputのEventに格納しOn（関数）を呼ばせるか　=>On(関数)の中で　switchなどで　ステータスに応じてはじいたり

}
