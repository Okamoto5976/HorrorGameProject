using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GetObjPlacementData", menuName = "Scriptable Objects/Data/GetObjPlacementData")]
public class GetObjPlacementData : ScriptableObject
{
    [System.Serializable]
    public class GetPlacementClass
    {
        public Enum_Stage m_stage;
        public Vector3 m_pos;
    }

    [SerializeField] private List<GetPlacementClass> m_list = new();

    public List<GetPlacementClass> List => m_list;
}
