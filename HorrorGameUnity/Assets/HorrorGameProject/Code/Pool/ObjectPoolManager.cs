using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private int m_count;
    [SerializeField] private GameObject m_generateObj;

    private Queue<GameObject> m_pool = new();

    private void Awake()
    {
        GeneratePool(m_count);
    }

    public GameObject GetObjectToPool()
    {
        if(m_pool.Count > 0)
        {
            var obj = m_pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return GenerateObject();
    }

    private void GeneratePool(int count)
    {
        for(int i = 0; i < count; i++)
        {
            var obj = GenerateObject();
            m_pool.Enqueue(obj);
        }
    }

    private GameObject GenerateObject()
    {
        GameObject obj = Instantiate(m_generateObj, transform.parent);

        obj.AddComponent<ReturnPool>();

        var returnPool = obj.GetComponent<ReturnPool>();

        if(returnPool != null )
        {
            returnPool.Init(this);
        }

        obj.SetActive(false);

        return obj;
    }

    public void ReturnPool(GameObject obj)
    {
        obj.SetActive(false);
        m_pool.Enqueue(obj);
    }
}
