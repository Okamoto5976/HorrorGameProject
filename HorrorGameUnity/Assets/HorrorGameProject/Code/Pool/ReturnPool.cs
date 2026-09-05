using UnityEngine;

public class ReturnPool : MonoBehaviour
{
    private ObjectPoolManager m_pool;

    public void Init(ObjectPoolManager pool)
    {
        m_pool = pool;
    }

    public void OnReturnPool()
    {
        m_pool.ReturnPool(this.gameObject);
    }
}
