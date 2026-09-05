using UnityEngine;

public class InteractRest : MonoBehaviour, IInteractable
{
    public void OnInteract(Player player)
    {
        player.OnInteractSafeProcess();

        //pos 
    }
}
