using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Enum_Enemy m_enemy;

    [SerializeField] private List<Enum_Stage> m_canStageList = new();

    [SerializeField] private int m_maxExistence;

    [SerializeField] private float m_frequencyTime = 1.0f;

    public Enum_Enemy Enemy => m_enemy;
    public List<Enum_Stage> CanStageList => m_canStageList;
    public int MaxExistence => m_maxExistence;
    public float FrequencyTime => m_frequencyTime;
}
