using UnityEngine;

[RequireComponent (typeof(Movement))]
[RequireComponent(typeof(PlayerInteract))]
[RequireComponent(typeof(PlayerStatus))]

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

    private enum StaminaState
    { 
        Idle,
        Move,
        Run,
        Resist,
        Hide,
        Rest,
        Safe
    }


    //Stateが普通の時にも　Interactができない時があるから
    private enum InteractState
    {
        Can,
        Not
    }

    private PlayerState m_playerState = PlayerState.Idle;

    public PlayerState State => m_playerState;

    //================================
    // Component References
    //================================

    private Movement m_playerMovement;

    private PlayerInteract m_playerInteract;

    private PlayerStatus m_playerStatus;

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


    //================================
    // Unity Methods
    //================================

    private void Awake()
    {
        m_playerMovement = GetComponent<Movement>();
        m_playerInteract = GetComponent<PlayerInteract>();
        m_playerStatus = GetComponent<PlayerStatus>();
        
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
        if (m_playerState == PlayerState.Safe)
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
        if(m_playerState == PlayerState.Safe ||
            m_playerState == PlayerState.Hide ||
            m_playerState == PlayerState.Resist ||
            m_playerState == PlayerState.Dead ||
            !m_canMove)
        {
            m_playerMovement.Move(Vector3.zero);

        }
        else
        {
            m_playerMovement.Move(m_inputDir);
        }



    }

    private void Resist(Vector2 dir)
    {
        if(!m_resistDir)
        {
            if(dir.x > 0)
            {
                m_playerStatus.AddResistValue(2f);
                m_resistDir = true;
            }
        }
        else
        {
            if(dir.x < 0)
            {
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
        if(m_playerState == PlayerState.Resist)
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
    }



    public void HitEnemy(EnemyBase component)
    {
        m_resistingEnemy = component;

        m_playerStatus.ResetResistValue();

        ChangeState(PlayerState.Resist);

    }

    public void KillPlayer()
    {
        ChangeState(PlayerState.Dead);
    }


    //================================
    // Change State Methods
    //================================

    private void ChangeState(PlayerState state)
    {
        if(m_playerState == state) return;

        m_playerState = state;
    }

    

    private void SetCanMove(bool isActive) => m_canMove = isActive;

   
     



    //OutSide Reference-----------------------------------------------
    public void ProcessSafe()
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

    public void ProcessHide()
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
