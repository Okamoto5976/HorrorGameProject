using UnityEngine;
/// <summary>
/// Use when UI or BGM
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource m_seSource;
    [SerializeField] private AudioSource m_bgmSource;

    //use fade process
    private bool m_isFadeOut = false;
    private float m_fadeOutSpeed = 0.9f;
    private float m_nextBgmVolume;
    private AudioClipData m_nextData;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySE(AudioClipData data)
    {
        m_seSource.PlayOneShot(data.Clip);
    }

    public void PlayBGM(AudioClipData data)
    {
        if(m_isFadeOut) return;

        if(m_bgmSource.isPlaying)
        {
            //Debug.LogWarning("isplaying");
            m_nextData = data;
            FadeOutBGM();
            return;
        }

        m_bgmSource.clip = data.Clip;
        m_bgmSource.Play();
    }

    public void StopBGM()
    {
        m_bgmSource.Stop();
    }

    private void FadeOutBGM()
    {
        m_nextBgmVolume = m_bgmSource.volume;
        m_isFadeOut = true;
    }

    private void Update()
    {
        if (!m_isFadeOut) return;

        m_bgmSource.volume -= Time.deltaTime * m_fadeOutSpeed;

        if(m_bgmSource.volume <= 0)
        {
            m_bgmSource.Stop();

            m_isFadeOut = false;

            m_bgmSource.volume = m_nextBgmVolume;
            PlayBGM(m_nextData);
        }

        
    }
}
