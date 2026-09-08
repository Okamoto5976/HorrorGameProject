using UnityEngine;

public class StartDisplay : MonoBehaviour
{
    public void OnStart()
    {
        LoadManager.Instance.OnMainLoad("MainScene");
    }
}
