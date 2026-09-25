using UnityEngine;

public class InteractHide : MonoBehaviour, IInteractable
{
    [SerializeField] private Enum_InteractObj m_interactObj;

    public void OnInteract(PlayerController player)
    {
        player.ProcessHide();

        //display dark

        //if player stamina no, sound

    }
}
