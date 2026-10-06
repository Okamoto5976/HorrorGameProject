using UnityEngine;

public class PlayerObari : MonoBehaviour
{
    private PlayerController m_playerController;
    private Movement m_movement;

    //Timerの初期化で　基礎値＋グローバルアラート
    [SerializeField] private float m_crushedTimer = 15f;
    private float m_timer;

    private bool m_active;

    private void Awake()
    {
        m_playerController = GetComponent<PlayerController>();
        m_movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (!m_active) return;

        m_timer -= Time.deltaTime;

        float timerRate = Mathf.Clamp01(m_timer / 15f);
        float obriValue = Mathf.Lerp(0.5f, 1f, timerRate);
        m_movement.SetObriValue(obriValue);

        if(m_timer <= 0)
        {
            Crushed();
        }
    }

    public void ActivationObri()
    {
        if(m_active) return;

        m_timer = m_crushedTimer;
        //Speedを徐々に下げる
        m_active = true;
        
    }

    private void Crushed()
    {
        //Player kill
        m_playerController.KillPlayer();
        m_active = false;

        m_movement.ResetObriValue();
    }

    public void Release()
    {
        m_active = false;

        m_movement.ResetObriValue();

    }
}
