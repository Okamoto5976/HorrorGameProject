using UnityEngine;

[RequireComponent (typeof(EnemyBase))]
[RequireComponent (typeof(Movement))]
public class EnemyGhost : MonoBehaviour
{
    //“®‚«•û‚ÉŠÉ‹}‚ğ‚Â‚¯‚é
    //–Ú“I@i˜H‚Ì–WŠQ
    //’èŠú“I‚ÉÁ‚¦‚é

    //================================
    // Component References
    //================================

    private EnemyBase m_enemyBase;
    private Movement m_movement;

    //get playerPos
    [SerializeField] private Vector3Asset m_playerPos;

    //[SerializeField] private float m_checkDistance;

    private Vector3 m_dir;

    private void Awake()
    {
        m_enemyBase = GetComponent<EnemyBase>();
        m_movement = GetComponent<Movement>();
    }

    private void Update()
    {
        DecideDir();

    }

    private void FixedUpdate()
    {
        if(m_enemyBase.State == EnemyBase.EnemyState.Attack ||
            m_enemyBase.State == EnemyBase.EnemyState.Disable)
        {
            m_movement.Move(Vector3.zero);

        }
        else
        {
            m_movement.Move(m_dir);

        }

    }

    private void DecideDir()
    {
        if (m_playerPos.Value.x < transform.position.x)
        {
            m_dir.x = -1f;
        }
        else
        {
            m_dir.x = 1f;
        }
    }
}
