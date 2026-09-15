using UnityEngine;

public class InteractToStage : MonoBehaviour, IInteractable
{
    private string m_viewName;

    [SerializeField] private Enum_Stage m_toStage;

    [SerializeField] private Enum_InteractObj m_interactObj;

    //public void Initialized(StageData data, Enum_Stage toStage)
    //{
    //    m_stageData = data;
    //    m_toStage = toStage;
    //}

    public Enum_InteractObj OnInteract(PlayerController player)
    {
        var currentStage = MainManager.Instance.CurrentStage;

        Vector3 position = MainManager.Instance.GetNextStageFromPoint(m_toStage, currentStage);

        MainManager.Instance.OnStageLoad(m_toStage, currentStage, player, position);

        return m_interactObj;
    }
}
