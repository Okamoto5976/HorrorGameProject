using UnityEngine;

public class EnemySurprise : MonoBehaviour
{
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private PlayerStatus m_playerStatus;

    [SerializeField] private float m_surpriseDecideTime;

    [SerializeField] private float m_deleteValue;

    private float m_surpriseTimer;

    private bool IsSurprise => m_surpriseTimer > m_surpriseDecideTime;

    public void UpdateTimer(bool value)
    {
        if(IsSurprise)
        {
            //event
            Debug.LogWarning("Enemy SurpriseÇ…èPÇÌÇÍÇΩ");

            m_playerController.SurpriseMap();
            m_playerStatus.DeleteStamina(m_deleteValue);

            m_surpriseTimer = 0f;
            return;
        }

        if(value)
        {
            m_surpriseTimer += Time.deltaTime;

        }
        else
        {
            m_surpriseTimer -= Time.deltaTime;
            m_surpriseTimer = Mathf.Max(0f, m_surpriseTimer);
        }
    }
}
