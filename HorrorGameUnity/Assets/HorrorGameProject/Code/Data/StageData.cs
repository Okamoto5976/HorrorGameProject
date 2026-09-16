using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/Data/StageData")]
public class StageData : ScriptableObject
{
    [System.Serializable]
    public class StageConnectPoint
    {
        public Enum_Stage m_connectStage;
        public Vector3 m_destinationPosition; //向こうの入り口
        public Vector3 m_entryPosition; //このステージからの入口

    }


    [SerializeField] private Enum_Stage m_mystage;

    [SerializeField] private List<StageConnectPoint> m_stageConnectPoint = new();

    [SerializeField] private string m_sceneName;

    public Enum_Stage MyStage => m_mystage;

    public List<StageConnectPoint> StageConnectPointList => m_stageConnectPoint;
    public string SceneName => m_sceneName;

    /// <summary>
    /// 次のステージの出現場所の取得
    /// </summary>
    /// <param name="nextStage"></param>
    /// <returns></returns>
    public Vector3 GetDestinationPositionOfNextStage(Enum_Stage nextStage)
    {
        //null check
        var point = m_stageConnectPoint.Find(x => x != null && x.m_connectStage == nextStage);

        if (point != null)
        {
            return point.m_destinationPosition;
        }
        else
        {
            Debug.LogWarning($"Null StageFromPoint");
        }

        return Vector3.zero;

    }
}
