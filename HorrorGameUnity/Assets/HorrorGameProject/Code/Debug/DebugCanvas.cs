using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text m_playerStageText;

    [SerializeField] private PlayerController m_playerController;

    private void Update()
    {
        m_playerStageText.text = "Player State : " + m_playerController.State.ToString();
    }

    [ContextMenu("HitEnemy")]
    public void HitEnemy()
    {
        //m_playerController.HitEnemy();
    }
}
