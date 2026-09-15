using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoadManager : MonoBehaviour
{
    private string m_nextSceneName;
    private string m_currentSceneName;

    private PlayerController m_player;
    private Vector3 m_pos;

    public void OnLoadScene(string nextScene, string currentScene, PlayerController player, Vector3 pos)
    {
        m_nextSceneName = nextScene;
        m_currentSceneName = currentScene;
        m_player = player;
        m_pos = pos;

        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        //string sceneName = SceneManager.GetActiveScene().name;

        yield return SceneManager.UnloadSceneAsync(m_currentSceneName);

        yield return SceneManager.LoadSceneAsync(m_nextSceneName, LoadSceneMode.Additive);

        m_player.transform.position = m_pos;

    }
}
