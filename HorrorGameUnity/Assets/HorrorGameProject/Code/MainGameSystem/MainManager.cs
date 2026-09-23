using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    [SerializeField] private PlayerController m_player;
    //[SerializeField] private GameObject m_map;

    [SerializeField] private string m_startScene;
    [SerializeField] private Enum_Stage m_startStage;
    [SerializeField] private Vector3 m_startPos;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        //MainInitialize();

        //m_player.SetMap(m_map);
    }

    //call gameStart
    public void MainInitialize()
    {
        var name = StageManager.Instance.GetSceneName(m_startStage);

        StartCoroutine(LoadSceneCoroutine(name));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        m_player.transform.position = m_startPos;

    }

    [SerializeField] private EnemyGenerater m_enemyGenerater;

    //call StageLoad
    public void InitStage(Enum_Stage nextStage)
    {
        GameManager.Instance.SetCurrentStage(nextStage);

        m_enemyGenerater.GenerateEnemy(nextStage);

        PlacementManager.Instance.SetObjectStage(nextStage);

    }


    //call load
    public IEnumerator LoadSaveDataCoroutine(GameSaveData data)
    {
        var name = StageManager.Instance.GetSceneName(data.stage);

        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        InitStage(data.stage);

        m_player.transform.position = data.pos;
    }

}
