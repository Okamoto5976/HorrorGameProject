using UnityEngine;

public class InteractGet : MonoBehaviour, IInteractable
{
    private Enum_InteractObj m_interactObj = Enum_InteractObj.Get;

    private bool m_isActive = true;

    public Enum_InteractObj OnInteract(PlayerController player)
    {
        if(GameManager.Instance.IsGameStart)
        {
            if (m_isActive)
            {
                GameManager.Instance.AddCollect();
                Debug.Log($"“ª‚ğ‰ñû‚µ‚½! Œ»İ:{GameManager.Instance.CollectHead}ŒÂ");

                m_isActive = false;
            }
            else
            {
                Debug.Log("‚·‚Å‚É‰ñû‚¸‚İ");
            }

        }

        return m_interactObj;
    }
}
