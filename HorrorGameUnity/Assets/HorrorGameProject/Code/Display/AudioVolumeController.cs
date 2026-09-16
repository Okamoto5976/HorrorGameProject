using UnityEngine;
using UnityEngine.Audio;
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

    private OptionSaveClass m_optionSaveClass;

    private void Awake()
    {
        m_optionSaveClass = new OptionSaveClass();
    }

    private void Start()
    {
        //get value from save
        var optionData = m_optionSaveClass.LoadOptionData();

        SetAudioMixer(optionData);

        //set value of slider
        SetValueOfSlider(m_masterName, m_masterSlider);
        SetValueOfSlider(m_bgmName, m_bgmSlider);
        SetValueOfSlider(m_seName, m_seSlider);
        SetValueOfSlider(m_voiceName, m_voiceSlider);

    }

    private void SetAudioMixer(OptionSaveData data)
    {
        if(data  != null)
        {
            m_audioMixer.SetFloat(m_masterName, data.m_masterVolume);
            m_audioMixer.SetFloat(m_bgmName, data.m_bgmVolume);
            m_audioMixer.SetFloat(m_seName, data.m_seVolume);
            m_audioMixer.SetFloat(m_voiceName, data.m_voiceVolume);
        }
        else
        {
            m_audioMixer.SetFloat(m_masterName, -0f);
            m_audioMixer.SetFloat(m_bgmName, -0f);
            m_audioMixer.SetFloat(m_seName, -0f);
            m_audioMixer.SetFloat(m_voiceName, -0f);

            Debug.Log("Initialize Set AudioVolume");
        }

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

        SaveOptionData();

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

        SaveOptionData();

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

        SaveOptionData();

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

        SaveOptionData();
    }

    private void SaveOptionData()
    {
        OptionSaveData data = new OptionSaveData();

        m_audioMixer.GetFloat(m_masterName, out float masterValue);
        data.m_masterVolume = masterValue;
        m_audioMixer.GetFloat(m_bgmName, out float bgmValue);
        data.m_bgmVolume = bgmValue;
        m_audioMixer.GetFloat(m_seName, out float seVolume);
        data.m_seVolume = seVolume;
        m_audioMixer.GetFloat(m_voiceName, out float voiceVolume);
        data.m_voiceVolume = voiceVolume;

        m_optionSaveClass.SaveOptionData(data);
    }
}
