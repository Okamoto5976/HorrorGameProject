using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolMediation : MonoBehaviour
{
    [System.Serializable]
    public class Mediation
    {
        public ObjectPoolManager m_pool;
        public Enum_Enemy m_enemyType;
    }

    [SerializeField] private List<Mediation> m_list;

    public GameObject GetPoolObject(Enum_Enemy enemyType)
    {
        var list = m_list.Find(x => x != null && x.m_enemyType == enemyType);

        if(list != null)
        {
            var obj = list.m_pool.GetObjectToPool();
            return obj;
        }

        return null;
    }
}
