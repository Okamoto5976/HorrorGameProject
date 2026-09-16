using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField] private PlayerController m_player;
    //[SerializeField] private GameObject m_map;

    [SerializeField] private string m_startScene;
    [SerializeField] private Enum_Stage m_startStage;
    [SerializeField] private Vector3 m_startPos;

    private void Start()
    {
        MainInitialize();

        //m_player.SetMap(m_map);
    }

    private void MainInitialize()
    {
        var name = StageManager.Instance.GetSceneName(m_startStage);

        StartCoroutine(LoadSceneCoroutine(name));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        m_player.transform.position = m_startPos;

    }
}
