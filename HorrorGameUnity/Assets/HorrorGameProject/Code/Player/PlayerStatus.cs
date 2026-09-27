using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public enum StaminaState
    {
        Idle,
        Move,
        Run,
        Resist,
        Hide,
        Rest,
        Safe
    }

    private StaminaState m_staminaState = StaminaState.Idle;


    //================================
    // Component References
    //================================
    private PlayerController m_playerController;

    //---runtime---------
    [SerializeField] private RuntimeFloat m_playerStamina;


    //----stamina----------------

    [SerializeField] private float m_maxStamina = 100f;
    private float m_stamina;
    private float m_staminaTime = 0f;

    [SerializeField] private Slider m_leftSlider;
    [SerializeField] private Slider m_rightSlider;

    private float m_gaugeVelocity = 0.1f;


    //----resist------------------
    [SerializeField] private Slider m_resistSlider;
    private float m_resistValue;



    //ステート重視で
    public float Stamina { private set { m_stamina = Mathf.Clamp(value, 0f, m_maxStamina); } get => m_stamina; }

    private void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        Stamina = m_maxStamina;
        m_resistSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(m_playerController.State == PlayerController.PlayerState.Resist)
        {
            m_resistSlider.value = m_resistValue;

            if (m_resistValue >= 20f)
            {
                m_resistValue = 0f;
                m_resistSlider.gameObject.SetActive(false);

                m_playerController.SuccessResist();
            }
            else if (m_resistValue < -20f)
            {
                m_resistValue = 0f;
                m_resistSlider.gameObject.SetActive(false);

                m_playerController.KillPlayer();
            }


            m_resistValue -= Time.deltaTime * 2f;
        }

        StaminaManager();

        UpdateStaminaSlider();
    }

    public void ResetResistValue()
    {
        m_resistValue = 0f;
        m_resistSlider.gameObject.SetActive(true);
    }

    public void AddResistValue(float value)
    {
        m_resistValue += value;
    }

    public void ChangeStaminaState(StaminaState state)
    {
        m_staminaState = state;
    }

    private void UpdateStaminaSlider()
    {
        float target = Stamina / m_maxStamina;

        m_rightSlider.value = Mathf.SmoothDamp(
            m_rightSlider.value,
            target,
            ref m_gaugeVelocity,
            0.5f);

        m_leftSlider.value = Mathf.SmoothDamp(
            m_leftSlider.value,
            target,
            ref m_gaugeVelocity,
            0.5f);
    }

    private void StaminaManager()
    {
        if(m_staminaState == StaminaState.Idle)
        {
            RecoverStamina(1f);
        }
        else if(m_staminaState == StaminaState.Move)
        {
            ConsumptionStamina(1f);
        }
        else if (m_staminaState == StaminaState.Run)
        {
            ConsumptionStamina(2f);
        }
        else if (m_staminaState == StaminaState.Rest)
        {
            RecoverStamina(2f);
        }
        else if (m_staminaState == StaminaState.Safe)
        {
            RecoverStamina(4f);
        }
    }

    public bool ResistStamina(float value)
    {
        if(m_stamina >= value)
        {
            m_stamina -= value;
            return true;
        }

        return false;
    }

    private void ConsumptionStamina(float multiply)
    {

        if (m_staminaTime >= 0.5f)
        {
            m_staminaTime = 0f;

            Stamina -= 1f * multiply;
        }

        m_staminaTime += Time.deltaTime;

    }

    private void RecoverStamina(float multiply)
    {
        if (m_staminaTime >= 0.5f)
        {
            m_staminaTime = 0f;

            Stamina += 1f * multiply;
        }

        m_staminaTime += Time.deltaTime;
    }

    
}
