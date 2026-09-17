using UnityEngine;

public class ReturnPool : MonoBehaviour
{
    private ObjectPoolManager m_pool;

    public void Init(ObjectPoolManager pool)
    {
        m_pool = pool;
    }

    public void CallReturnPool()
    {
        m_pool.ReturnPool(this.gameObject);
    }
}
