using System.Collections;
using UnityEngine;

public class InteractRest : MonoBehaviour, IInteractable
{
    
    private Enum_InteractObj m_interactObj = Enum_InteractObj.Safe;

    public Enum_InteractObj OnInteract(PlayerController player)
    {
        if (!GameManager.Instance.IsGameStart)
        {
            Debug.LogWarning("まだSafeが起動していない");

            return m_interactObj;
        }

        //player.OnInteractSafeProcess();

        //pos 

        StartCoroutine(SaveGameData());

        return m_interactObj;
    }

    private IEnumerator SaveGameData()
    {
        Debug.LogWarning("セーブ中");

        GameSaveData data = new();

        data.pos = transform.position;

        data = GameManager.Instance.SaveGameData(data);

        data = PlacementManager.Instance.SaveGameData(data);

        GameSaveClass saveClass = new();

        saveClass.SaveGameData(data);

        Debug.LogWarning("Saveが完了しました。");

        yield return null;
    }
}
