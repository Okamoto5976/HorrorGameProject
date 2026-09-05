using UnityEngine;

public class InteractHide : MonoBehaviour, IInteractable
{
    public void OnInteract(Player player)
    {
        player.OnInteractHideProcess();

        //display dark

        //if player stamina no, sound
    }
}
