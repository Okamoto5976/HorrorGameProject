using UnityEngine;

public class InteractGet : MonoBehaviour, IInteractable
{
    private Enum_InteractObj m_interactObj = Enum_InteractObj.Get;

    private bool m_isGet;

    public Enum_InteractObj OnInteract(PlayerController player)
    {
        if(GameManager.Instance.IsGameStart)
        {
            if (!m_isGet)
            {
                GameManager.Instance.AddCollect();
                Debug.Log($"“ª‚ğ‰ñû‚µ‚½! Œ»İ:{GameManager.Instance.CollectHead}ŒÂ");

                var currentStage = GameManager.Instance.CurrentStage;

                PlacementManager.Instance.UpdateData(currentStage);

                m_isGet = true;
            }
            else
            {
                Debug.Log("‚·‚Å‚É‰ñû‚¸‚İ");
            }

        }

        return m_interactObj;
    }

    public void Initialized(bool value)
    {
        m_isGet = value;
    }
}
