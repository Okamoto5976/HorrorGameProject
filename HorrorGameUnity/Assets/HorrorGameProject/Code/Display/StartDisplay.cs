using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class StartDisplay : MonoBehaviour
{
    [Header("Option Reference")]
    [SerializeField] private GameObject m_option;

    //[SerializeField] private AudioMixer m_mixer;
    //[SerializeField] private Slider m_masterSlider;
    //[SerializeField] private Slider m_bgmSlider;
    //[SerializeField] private Slider m_seSlider;
    //[SerializeField] private Slider m_voiceSlider;

    //private string m_masterVolume = "_MasterVolume";
    //private string m_bgmVolume = "_BGMVolume";
    //private string m_seVolume = "_SEVolume";
    //private string m_voiceVolume = "_VoiceVolume";


    //private OptionSaveClass m_optionSaveClass; 

    //private void Awake()
    //{
    //    m_optionSaveClass = new();   
    //}

    //private void Start()
    //{
    //    var optionData = m_optionSaveClass.LoadOptionData();

    //    m_mixer.SetFloat()
    //}

    public void OnStart()
    {
        LoadManager.Instance.OnMainLoad("MainScene");
    }

    public void OnLoad()
    {

    }

    public void OnOption()
    {
        m_option.SetActive(true);
    }

    public void CloseOption()
    {
        m_option.SetActive(false);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
