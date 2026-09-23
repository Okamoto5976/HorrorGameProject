using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoadSystem : MonoBehaviour
{
    private string m_nextSceneName;
    private string m_currentSceneName;

    private PlayerController m_player;
    private Vector3 m_pos;

    private Enum_Stage m_nextStage;

    [SerializeField] private MainManager m_mainManager;


    public void OnLoadScene(Enum_Stage nextStage, PlayerController player, Vector3 pos)
    {
        var currentStage = GameManager.Instance.CurrentStage;

        m_nextSceneName = StageManager.Instance.GetSceneName(nextStage);
        m_currentSceneName = StageManager.Instance.GetSceneName(currentStage);
        m_player = player;
        m_pos = pos;

        m_nextStage = nextStage;

        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        //string sceneName = SceneManager.GetActiveScene().name;

        yield return SceneManager.UnloadSceneAsync(m_currentSceneName);

        yield return SceneManager.LoadSceneAsync(m_nextSceneName, LoadSceneMode.Additive);

        m_player.transform.position = m_pos;

        m_mainManager.InitStage(m_nextStage);
    }
}
