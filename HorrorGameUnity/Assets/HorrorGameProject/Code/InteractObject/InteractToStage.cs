using UnityEngine;

public class InteractToStage : MonoBehaviour, IInteractable
{
    private string m_viewName;

    [SerializeField] private Enum_Stage m_nextStage;

    private Enum_InteractObj m_interactObj = Enum_InteractObj.MoveStage;

    //public void Initialized(StageData data, Enum_Stage toStage)
    //{
    //    m_stageData = data;
    //    m_toStage = toStage;
    //}

    public Enum_InteractObj OnInteract(PlayerController player)
    {
        var currentStage = GameManager.Instance.CurrentStage;

        Vector3 position = StageManager.Instance.GetDestinationPositionFromList(m_nextStage, currentStage);

        StageManager.Instance.OnStageLoad(m_nextStage, player, position);

        return m_interactObj;
    }
}
