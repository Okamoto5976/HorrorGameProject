using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EventData", menuName = "Scriptable Objects/Data/EventData")]
public class EventData : ScriptableObject
{
    [System.Serializable]
    public class EventClass
    {
        public Enum_Stage m_stage;
        public Vector3 m_pos;
    }

    [SerializeField] private Enum_Event m_eventType;

    [SerializeField] private List<EventClass> m_posList = new();

    public Enum_Event EventType => m_eventType;

    public List<EventClass> PosList => m_posList;
}
