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

    [SerializeField] private EnemyManager m_enemyManager;

    [SerializeField] private int m_clearCount;


    private Enum_Stage m_currentStage;
    public Enum_Stage CurrentStage => m_currentStage;



    private int m_collectHead;
    public int CollectHead => m_collectHead;

    [SerializeField] private float m_gameTimer = 120f;
    public float GameTimer => m_gameTimer;

    [SerializeField] private float m_globalAlert;
    public float GlobalAlert => m_globalAlert;

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

        CalculationAlert(-0.5f * Time.deltaTime);
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

        m_enemyManager.GenerateEnemy(m_collectHead);
    }

    public void CalculationAlert(float value)
    {
        m_globalAlert += value;
        m_globalAlert = Mathf.Clamp(m_globalAlert, 0f, 100f);
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
        data.gameTime = m_gameTimer;
        data.collect = m_collectHead;
        data.currentStage = m_currentStage;

        return data;
    }

    public void SetGameData(GameSaveData data)
    {
        m_gameTimer = data.gameTime;
        m_collectHead = data.collect;

        m_isGameStart = true;
    }
}
