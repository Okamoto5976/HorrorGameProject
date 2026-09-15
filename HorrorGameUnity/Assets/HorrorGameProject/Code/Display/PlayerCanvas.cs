using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{

    //GameObject Reference

    [SerializeField] private GameObject m_mapCanvas;


    //variable
    private bool m_isActive = false;

    private void Start()
    {
        m_mapCanvas.SetActive(m_isActive);
    }

    public bool IsActiveMapCanvas()
    {
        m_isActive = !m_isActive;

        m_mapCanvas.SetActive(m_isActive);

        return m_isActive;
    }
}
