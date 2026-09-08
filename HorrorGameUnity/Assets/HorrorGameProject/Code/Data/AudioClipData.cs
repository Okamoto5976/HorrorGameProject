using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipData", menuName = "Scriptable Objects/Data/AudioClipData")]
public class AudioClipData : ScriptableObject
{
    [SerializeField] private AudioClip m_clip;

    public AudioClip Clip => m_clip;
}
