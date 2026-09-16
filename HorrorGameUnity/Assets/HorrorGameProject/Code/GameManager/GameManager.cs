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
    public int ClearCount => m_clearCount;


    private Enum_Stage m_currentStage;
    public Enum_Stage CurrentStage => m_currentStage;



    private int m_collectHead;
    public int CollectHead => m_collectHead;

    public void SetCurrentStage(Enum_Stage stage) => m_currentStage = stage;

    public void GameOver()
    {
        //player dead
        //UI view
        //
    }

    public void GameClear()
    {
        //end 
        //event
    }
}
