using UnityEngine;

[RequireComponent (typeof(Movement))]
public class EnemyMimic : EnemyBase, IInteractable
{
    //================================
    // Component References
    //================================

    private Movement m_movement;
    private SpriteRenderer m_renderer;

    //================================
    //î•ñŽæ“¾
    //================================
    //get playerPos
    [SerializeField] private Vector3Asset m_playerPos;

    [SerializeField] private Vector3Asset m_playerFacingDir;

    [SerializeField] private RuntimeFloat m_playerStamina;

    //Distance
    private float m_sqrDistance => (m_playerPos.Value - transform.position).sqrMagnitude;

    //==============================
    //Flag
    //==============================


    [SerializeField] private float m_checkDiscoveryDistance;
    public float CheckDis => m_checkDiscoveryDistance;

    //==============================
    //Debug
    //==============================

    [SerializeField] private RuntimeBool m_debugMode;

    [SerializeField] private TMPro.TextMeshPro m_debugText;

    //==============================
    // Unity
    //==============================

    private void Awake()
    {
        m_movement = GetComponent<Movement>();
        m_renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        
    }

    protected override void Init()
    {
        
    }

    private void Update()
    {
        if (m_debugMode.Value)
        {
            m_debugText.text = "Mimic";
        }

        if (m_sqrDistance <= CheckDis * CheckDis)
        {
            //se
        }
    }

    public void OnInteract(PlayerController player)
    {
        Debug.Log("Mimic");
        ChangeState(EnemyState.Idle);
    }

}
