using System.Collections;
using UnityEngine;

public class InteractSafe : MonoBehaviour, IInteractable
{
    
    private Enum_InteractObj m_interactObj = Enum_InteractObj.Safe;

    public void OnInteract(PlayerController player)
    {
        if (!GameManager.Instance.IsGameStart)
        {
            Debug.LogWarning("まだSafeが起動していない");

            return;
        }

        player.ProcessSafe();

        //pos 

        StartCoroutine(SaveGameData());

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
