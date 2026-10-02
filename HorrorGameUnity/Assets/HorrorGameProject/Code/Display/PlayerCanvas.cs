using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{

    //GameObject Reference

    [SerializeField] private GameObject m_mapCanvas;


    //variable
    private bool m_isActive = false;

    [SerializeField] private EnemySurprise m_enemySurprise;

    private void Start()
    {
        m_mapCanvas.SetActive(m_isActive);
    }

    private void Update()
    {
        m_enemySurprise.UpdateTimer(m_isActive);
    }

    public bool IsActiveMapCanvas()
    {
        m_isActive = !m_isActive;

        m_mapCanvas.SetActive(m_isActive);

        return m_isActive;
    }
}
