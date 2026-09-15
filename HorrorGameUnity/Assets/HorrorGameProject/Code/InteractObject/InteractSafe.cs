using UnityEngine;

public class InteractRest : MonoBehaviour, IInteractable
{
    
    [SerializeField] private Enum_InteractObj m_interactObj;

    public Enum_InteractObj OnInteract(PlayerController player)
    {
        //player.OnInteractSafeProcess();

        //pos 

        return m_interactObj;
    }
}
