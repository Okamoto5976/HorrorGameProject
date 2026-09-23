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

    //call frome shrine
    public void StartGame()
    {
        m_isGameStart = true;

        //Enemy Generate Start
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

    //Save process------------------------------

    public GameSaveData SaveGameData(GameSaveData data)
    {
        data.time = m_gameTimer;
        data.collect = m_collectHead;
        data.stage = m_currentStage;

        return data;
    }

    public void SetGameData(GameSaveData data)
    {
        m_gameTimer = data.time;
        m_collectHead = data.collect;

        m_isGameStart = true;
    }
}
