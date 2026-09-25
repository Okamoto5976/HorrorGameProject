using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    //================================
    // Component References
    //================================
    private PlayerController m_playerController;


    //----stamina----------------

    [SerializeField] private float m_maxStamina = 100f;
    private float m_stamina;
    private float m_staminaTime = 0f;

    [SerializeField] private Slider m_leftSlider;
    [SerializeField] private Slider m_rightSlider;

    private float m_gaugeVelocity = 0.2f;


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

       

        UpdateStaminaSlider();

        ConsumptionStamina(1f);
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

    private void UpdateStaminaSlider()
    {
        float target = Stamina / m_maxStamina;

        m_rightSlider.value = Mathf.SmoothDamp(
            m_rightSlider.value,
            target,
            ref m_gaugeVelocity,
            1f);

        m_leftSlider.value = Mathf.SmoothDamp(
            m_leftSlider.value,
            target,
            ref m_gaugeVelocity,
            1f);
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
