using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainSceneLoadingSystem : MonoBehaviour
{
    [SerializeField] private PlayerController m_player;
    [SerializeField] private GameObject m_map;

    [SerializeField] private string m_startScene;
    [SerializeField] private Vector3 m_startPos;

    private void Start()
    {
        MainInitialize();

        //m_player.SetMap(m_map);
    }

    private void MainInitialize()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        yield return SceneManager.LoadSceneAsync(m_startScene, LoadSceneMode.Additive);

        m_player.transform.position = m_startPos;

    }
}
