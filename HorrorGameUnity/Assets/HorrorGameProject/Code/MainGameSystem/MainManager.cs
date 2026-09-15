using UnityEngine;
using System.Collections.Generic;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    [SerializeField] private List<StageData> m_stageDatas;

    private Enum_Stage m_currentStage;

    public Enum_Stage CurrentStage => m_currentStage;

    private StageLoadManager m_stageLoadManager;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        m_stageLoadManager = GetComponent<StageLoadManager>();
    }

    public void SetCurrentStage(Enum_Stage stage)
    {
        m_currentStage = stage;
    }

    /// <summary>
    /// 次のステージの初期位置を取得する
    /// </summary>
    /// <param name="nextStage">どのステージに行くか</param>
    /// <param name="nowStage">今どのステージにいるか</param>
    /// <returns>次のステージのスポーン場所</returns>
    public Vector3 GetNextStageFromPoint(Enum_Stage nextStage, Enum_Stage nowStage)
    {
        var stage = m_stageDatas.Find(x => x != null && x.MyStage == nextStage);

        if(stage != null)
        {
            return stage.GetStageFromPosition(nowStage);
        }
        else
        {
            Debug.LogError("Null StageData in StageDatas");
        }

        return Vector3.zero;
    }

    public void OnStageLoad(Enum_Stage nextStage, Enum_Stage nowStage, PlayerController player, Vector3 pos)
    {
        string nextSceneName = GetStageSceneName(nextStage);
        string currentSceneName = GetStageSceneName(nowStage);

        m_stageLoadManager.OnLoadScene(nextSceneName, currentSceneName, player, pos);
    }

    private string GetStageSceneName(Enum_Stage stage)
    {
        var data = m_stageDatas.Find(x => x != null && x.MyStage == stage);

        if (data != null)
        {
            return data.SceneName;
        }
        else
        {
            Debug.LogError("Null StageData in StageDatas");
        }

        return null;
    }
}
