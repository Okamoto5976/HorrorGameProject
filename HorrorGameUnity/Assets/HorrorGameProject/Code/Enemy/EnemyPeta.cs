using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
[RequireComponent(typeof(Movement))]
public class EnemyPeta : MonoBehaviour
{
    //敵が止まった時少しとまる
    //通り過ぎる間　振り向いたら襲う

    //================================
    // Component References
    //================================

    private EnemyBase m_enemyCommon;
    private Movement m_movement;

    //get playerPos
    [SerializeField] private Vector3Asset m_playerPos;

    [SerializeField] private float m_checkDistance;

    private Vector3 m_dir;

    private void Awake()
    {
        m_enemyCommon = GetComponent<EnemyBase>();
        m_movement = GetComponent<Movement>();
    }

    private void Update()
    {
        //開始時　プレイヤー
    }
}
