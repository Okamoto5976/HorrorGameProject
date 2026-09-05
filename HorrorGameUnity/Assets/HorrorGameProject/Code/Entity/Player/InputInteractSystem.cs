using System.Linq;
using UnityEngine;

public class InputInteractSystem
{
    public void TryInteract(Vector3 position, LayerMask layer, Player player)
    {
        //Debug.Log("Call TryInteract");

        Collider[] colliders = Physics.OverlapSphere(position, 2f, layer);

        //orderByDistance
        var orderedByProximity = colliders.OrderBy(c => (position - c.transform.position).sqrMagnitude).ToArray();

        //Debug.Log($"Interact one : {orderedByProximity[0].gameObject.name}  two : {orderedByProximity[1].gameObject.name}");

        if (orderedByProximity[0].TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.OnInteract(player);
        }
    }
}
