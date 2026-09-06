using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject m_player;

    [SerializeField] private Vector3 m_offset;

    private Vector3 m_target;

    private void Start()
    {

        m_offset = transform.position - m_player.transform.position;
    }

    private void LateUpdate()
    {
        m_target = m_player.transform.position;

        transform.position = m_target + m_offset;
    }
}
