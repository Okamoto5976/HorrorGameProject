using UnityEngine;

public class DebugMainSystem : MonoBehaviour
{
    [SerializeField] private RuntimeBool m_debugMode;

    void Start()
    {
        if (!m_debugMode.Value) return;

        Debug.LogWarning("GetObjectの初期化、ランダム設置");
        PlacementManager.Instance.Initialized();

        Debug.LogWarning("Eventランダム設定");
        EventManager.Instance.Initialized();

        Debug.LogWarning("マップ生成　初期化");
        MainManager.Instance.MainInitialize();
    }
}
