using UnityEngine;

public class InteractShrine : MonoBehaviour, IInteractable
{
    private Enum_InteractObj m_interactObj = Enum_InteractObj.Shrine;

    public Enum_InteractObj OnInteract(PlayerController player)
    {
        if(!GameManager.Instance.IsGameStart)
        {
            //Call Manager "Game Start"
            GameManager.Instance.StartGame();
            Debug.LogWarning("Game Start");
        }
        else
        {
            if(!GameManager.Instance.CheckCollict())
            {
                Debug.Log("“ª‚Ì”‚ª‘«‚è‚È‚¢");
            }
        }



        return m_interactObj;
    }
}
