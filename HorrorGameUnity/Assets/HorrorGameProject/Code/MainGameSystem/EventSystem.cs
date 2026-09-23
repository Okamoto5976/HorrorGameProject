using UnityEngine;

public class EventSystem : MonoBehaviour
{
    //Eventの確率を出す（Dataから　確率のデータを貰う）
    //個別のスクリプトに　発動可能　かどうかを渡す

    private IEvent m_eventComponent;

    private void Awake()
    {
        m_eventComponent = GetComponent<IEvent>();
    }

    private void Start()
    {
        if(m_eventComponent != null)
        {
            m_eventComponent.SetActive(true);

        }
    }
}
