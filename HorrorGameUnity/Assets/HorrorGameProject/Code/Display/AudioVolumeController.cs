using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioVolumeController : MonoBehaviour
{
    //slider 0~1 mixer -80 ~ 20

    [SerializeField] private AudioMixer m_audioMixer;
    [SerializeField] private Slider m_masterSlider;
    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_seSlider;
    [SerializeField] private Slider m_voiceSlider;

    private string m_masterName = "Master_Volume";
    private string m_bgmName = "BGM_Volume";
    private string m_seName = "SE_Volume";
    private string m_voiceName = "Voice_Volume";



    private void Start()
    {
        TestSet();
        //get value from save

        //set value of slider
        SetValueOfSlider(m_masterName, m_masterSlider);
        SetValueOfSlider(m_bgmName, m_bgmSlider);
        SetValueOfSlider(m_seName, m_seSlider);
        SetValueOfSlider(m_voiceName, m_voiceSlider);

    }

    private void TestSet()
    {
        m_audioMixer.SetFloat(m_masterName, -0f);
        m_audioMixer.SetFloat(m_bgmName, -0f);
        m_audioMixer.SetFloat(m_seName, -0f);
        m_audioMixer.SetFloat(m_voiceName, -0f);

    }

    private void SetValueOfSlider(string name, Slider slider)
    {
        float db;

        m_audioMixer.GetFloat(name, out db);

        float volume = Mathf.Pow(10f, db / 20f);

        slider.value = volume;
    }

    //1f = 0dB 0.01f = -40dB
    public void SetMasterVolume(float  volume)
    {
        if (volume <= 0f)
        {
            m_audioMixer.SetFloat(m_masterName, -80f); // –³‰¹ˆµ‚¢
        }
        else
        {
            m_audioMixer.SetFloat(m_masterName, Mathf.Log10(volume) * 20);
        }
    }

    public void SetBGMVolume(float volume)
    {
        if (volume <= 0f)
        {
            m_audioMixer.SetFloat(m_bgmName, -80f); // –³‰¹ˆµ‚¢
        }
        else
        {
            m_audioMixer.SetFloat(m_bgmName, Mathf.Log10(volume) * 20);
        }
    }

    public void SetSEVolume(float volume)
    {
        if (volume <= 0f)
        {
            m_audioMixer.SetFloat(m_seName, -80f); // –³‰¹ˆµ‚¢
        }
        else
        {
            m_audioMixer.SetFloat(m_seName, Mathf.Log10(volume) * 20);
        }
    }

    public void SetVoiceVolume(float volume)
    {
        if (volume <= 0f)
        {
            m_audioMixer.SetFloat(m_voiceName, -80f); // –³‰¹ˆµ‚¢
        }
        else
        {
            m_audioMixer.SetFloat(m_voiceName, Mathf.Log10(volume) * 20);
        }
    }
}
