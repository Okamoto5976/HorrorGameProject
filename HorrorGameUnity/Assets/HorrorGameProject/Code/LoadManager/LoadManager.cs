using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;

    [SerializeField] private GameObject m_panel;

    [SerializeField] private CanvasGroup m_canvasGroup;

    private float m_fadeTimer = 1f;

    private GameSaveClass m_gameSaveClass;

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

    public void OnStart(string sceneName)
    {
        StartCoroutine(ToMainGameStartCoroutine(sceneName));
    }

    public void OnLoad(string sceneName)
    {
        m_gameSaveClass = new();

        if(m_gameSaveClass.CheckSaveData())
        {
            StartCoroutine(ToMainGameLoadCoroutine(sceneName));
        }
        else
        {
            Debug.LogWarning("SaveDataがありません");
        }

    }

    private IEnumerator ToMainGameStartCoroutine(string sceneName)
    {
        m_canvasGroup.blocksRaycasts = true;

        yield return StartCoroutine(FadeCanvas(0f, 1f));

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

        Debug.LogWarning("GetObjectの初期化、ランダム設置");
        PlacementManager.Instance.Initialized();

        Debug.LogWarning("Eventランダム設定");
        EventManager.Instance.Initialized();

        Debug.LogWarning("マップ生成　初期化");
        MainManager.Instance.MainInitialize();

        yield return StartCoroutine(FadeCanvas(1f, 0f));

        m_canvasGroup.blocksRaycasts = false;
    }


    private IEnumerator ToMainGameLoadCoroutine(string sceneName)
    {
        m_canvasGroup.blocksRaycasts = true;

        yield return StartCoroutine(FadeCanvas(0f, 1f));

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;

        }
        var data = m_gameSaveClass.LoadGameData();



        //gameData set
        Debug.LogWarning("GameProgress 読み込み");
        GameManager.Instance.SetGameData(data);

        //enemy set
        //Placement set
        Debug.LogWarning("GetObject PlacementData 読み込み");
        PlacementManager.Instance.SetGameData(data);

        //event set

        //MainManagerでPlayerPosを変更　＝＞　休憩にもっていきたい

        //mainManager set stage読み込み + PlayerPos
        Debug.LogWarning("Stage 読み込み");
        yield return StartCoroutine(MainManager.Instance.LoadSaveDataCoroutine(data));

        yield return StartCoroutine(FadeCanvas(1f, 0f));

        m_canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator FadeCanvas(float start, float end)
    {
        float time = 0f;

        m_canvasGroup.alpha = start;

        while (time < m_fadeTimer)
        {
            time += Time.deltaTime;

            float t = time / m_fadeTimer;
            m_canvasGroup.alpha = Mathf.Lerp(start, end, t);

            yield return null;
        }

        m_canvasGroup.alpha = end;
    }
    
}
