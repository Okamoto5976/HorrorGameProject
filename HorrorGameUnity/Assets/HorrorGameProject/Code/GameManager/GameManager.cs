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

    private int m_collectHead;

    //propaty
    public int ClearCount => m_clearCount;
    public int CollectHead => m_collectHead;


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
