using UnityEngine;

public class TestAudio : MonoBehaviour
{
    [SerializeField] private AudioClipData m_bgm1Data;
    [SerializeField] private AudioClipData m_bgm2Data;

    [ContextMenu("PlayOne")]
    public void OnPlayOne()
    {
        AudioManager.Instance.PlayBGM(m_bgm1Data);
    }

    [ContextMenu("PlayTwo")]
    public void OnPlayTwo()
    {
        AudioManager.Instance.PlayBGM(m_bgm2Data);

    }

    public void OnStopBGM()
    {
        AudioManager.Instance.StopBGM();
    }
}
