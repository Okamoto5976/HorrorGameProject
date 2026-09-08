using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnMainLoad(string sceneName)
    {
        StartCoroutine(ToMainGameLoadCoroutine(sceneName));
    }

    private IEnumerator ToMainGameLoadCoroutine(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if(progress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;

        }
    }
}
