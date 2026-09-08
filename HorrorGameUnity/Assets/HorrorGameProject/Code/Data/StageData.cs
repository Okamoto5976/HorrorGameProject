using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/Data/StageData")]
public class StageData : ScriptableObject
{
    [System.Serializable]
    public class StageConnectPoint
    {
        public Enum_Stage m_connectStage;
        public Vector3 m_toPosition; //そのシーンに行くPosition
        public Vector3 m_fromPosition; //そのシーンから来るPosition

    }


    [SerializeField] private Enum_Stage m_mystage;

    [SerializeField] private List<StageConnectPoint> m_stageConnectPoint = new();

    [SerializeField] private string m_sceneName;

    public Enum_Stage MyStage => m_mystage;

    public List<StageConnectPoint> StageConnectPointList => m_stageConnectPoint;
    public string SceneName => m_sceneName;

    //どこから
    public Vector3 GetStageFromPosition(Enum_Stage from)
    {
        //null check
        var point = m_stageConnectPoint.Find(x => x != null && x.m_connectStage == from);

        if (point != null)
        {
            return point.m_fromPosition;
        }
        else
        {
            Debug.LogWarning($"Null StageFromPoint");
        }

        return Vector3.zero;

    }
}
