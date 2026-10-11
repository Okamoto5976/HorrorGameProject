using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyGenerateTiming
{
    //collect ‚Ì”
    public int collectNum;

    public List<EnemyGenerateNum> list = new();
}

[System.Serializable]
public struct EnemyGenerateNum
{
    public Enum_Enemy enemyType;
    public int num;
}

[CreateAssetMenu(fileName = "EnemyGenerateData", menuName = "Scriptable Objects/Data/EnemyGenerateData")]
public class EnemyGenerateData : ScriptableObject
{
    //“G‚²‚Æ‚É@num@‚É‰‚¶‚Ä‚Ì¶¬”
    [SerializeField] private List<EnemyGenerateTiming> m_enemyGenerateList = new();

    public List<EnemyGenerateNum> GetEnemyGenerateList(int num)
    {
        var list = m_enemyGenerateList.Find(x => x != null && x.collectNum == num).list;

        return list;
    }
}
