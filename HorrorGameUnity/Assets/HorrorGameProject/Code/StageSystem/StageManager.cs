using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [SerializeField] private List<StageData> m_stageDatas;

    private StageLoadSystem m_stageLoadManager;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        m_stageLoadManager = GetComponent<StageLoadSystem>();
    }

    /// <summary>
    /// 次のステージの初期位置を取得する
    /// </summary>
    /// <param name="nextStage">どのステージに行くか</param>
    /// <param name="nowStage">今どのステージにいるか</param>
    /// <returns>次のステージのスポーン場所</returns>
    public Vector3 GetDestinationPositionFromList(Enum_Stage nextStage, Enum_Stage nowStage)
    {
        var stage = m_stageDatas.Find(x => x != null && x.MyStage == nowStage);

        if(stage != null)
        {
            return stage.GetDestinationPositionOfNextStage(nextStage);
        }
        else
        {
            Debug.LogError("Null StageData in StageDatas");
        }

        return Vector3.zero;
    }

    /// <summary>
    /// ステージのシーン名を取得する
    /// </summary>
    public string GetSceneName(Enum_Stage stage)
    {
        var data = m_stageDatas.Find(x => x != null && x.MyStage == stage);

        if(data != null)
        {
            return data.SceneName;
        }
        else
        {
            Debug.LogError("Null StageData in StageDatas");

        }

        return null;

    }

    public void OnStageLoad(Enum_Stage nextStage, PlayerController player, Vector3 pos)
    {
        m_stageLoadManager.OnLoadScene(nextStage, player, pos);
    }

    /// <summary>
    /// 渡したステージの　渡した敵が出現できるポジションを返す
    /// </summary>
    /// <returns></returns>
    public EnemySpawnPosition GetEnemySpawnPosition(Enum_Stage stage, Enum_Enemy enemy)
    {
        var data = GetStageData(stage);

        return data.GetEnemySpawnPos(enemy);
    }

    /// <summary>
    /// StageDataを返す
    /// </summary>
    /// <returns></returns>
    public StageData GetStageData(Enum_Stage stage)
    {
        return m_stageDatas.Find(x => x != null && x.MyStage == stage);
    }
}
