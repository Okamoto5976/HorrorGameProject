using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    [SerializeField] private int m_clearCount;


    private Enum_Stage m_currentStage;
    public Enum_Stage CurrentStage => m_currentStage;



    private int m_collectHead;
    public int CollectHead => m_collectHead;

    [SerializeField] private float m_gameTimer = 120f;
    public float GameTimer => m_gameTimer;

    private bool m_isGameStart = false;

    public bool IsGameStart => m_isGameStart;

    public void SetCurrentStage(Enum_Stage stage) => m_currentStage = stage;

    private void Update()
    {
        if(m_isGameStart)
        {
            CountTimer();

        }

        if(m_gameTimer <= 0f)
        {
            GameOver();
        }
    }

    private void CountTimer()
    {
        m_gameTimer -= Time.deltaTime;
    }

    public void StartGame()
    {
        m_isGameStart = true;
    }

    public void AddCollect()
    {
        m_collectHead++;
    }

    public bool CheckCollict()
    {
        if(m_collectHead >= m_clearCount)
        {
            GameClear();
            return true;
        }

        return false;
    }

    public void GameOver()
    {
        Debug.LogWarning("Game Over");

        //player dead
        //UI view
        //
    }

    public void GameClear()
    {
        //end 
        //event

        Debug.LogWarning("Game Clear");
    }
}
