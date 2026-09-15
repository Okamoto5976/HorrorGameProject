using UnityEngine;

public class InteractHide : MonoBehaviour, IInteractable
{
    [SerializeField] private Enum_InteractObj m_interactObj;

    public Enum_InteractObj OnInteract(PlayerController player)
    {
        //player.OnInteractHideProcess();

        //display dark

        //if player stamina no, sound

        return m_interactObj;
    }
}
