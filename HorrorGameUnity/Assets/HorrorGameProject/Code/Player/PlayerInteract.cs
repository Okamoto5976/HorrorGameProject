using System.Linq;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    //================================
    // Component References
    //================================

    private PlayerController m_playerController;



    [SerializeField] private LayerMask m_interactLayer;


    private void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
    }

    public void TryInteract(PlayerController player)
    {
        //Debug.Log("Call TryInteract");

        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, m_interactLayer);

        //orderByDistance
        var orderedByProximity = colliders.OrderBy(c => (transform.position - c.transform.position).sqrMagnitude).ToArray();

        //Debug.Log($"Interact one : {orderedByProximity[0].gameObject.name}  two : {orderedByProximity[1].gameObject.name}");

        if (orderedByProximity.Length < 0) return;

        if (orderedByProximity[0].TryGetComponent<IInteractable>(out var interactable))
        {
            var type = interactable.OnInteract(player);

            ChangeState(type);
        }
    }

    private void ChangeState(Enum_InteractObj type)
    {
        switch(type)
        {
            case Enum_InteractObj.MoveStage:
                break;
            case Enum_InteractObj.Safe:
                m_playerController.ProcessHide();

                break;
            case Enum_InteractObj.Hide:
                m_playerController.ProcessHide();

                break;
            case Enum_InteractObj.Research:

                break;
        }
    }
}
