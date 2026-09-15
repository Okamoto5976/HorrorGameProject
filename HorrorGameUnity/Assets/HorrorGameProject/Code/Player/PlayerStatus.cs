using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float m_stamina = 100f;
    private float m_staminaTime = 0f;

    //ステート重視で
    public float Stamina { private set { m_stamina = Mathf.Clamp(value, 0f, 100f); } get => m_stamina; }

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
