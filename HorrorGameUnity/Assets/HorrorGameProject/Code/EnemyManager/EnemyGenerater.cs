using UnityEngine;
using System.Collections.Generic;

public class EnemyGenerater : MonoBehaviour
{
    [SerializeField] private EnemyManager m_enemyManager;

    [SerializeField] private EnemyPoolMediation m_enemyPoolMediation;

    private List<EnemyController> m_enemyList = new();

    //call Move Scene
    public void GenerateEnemy(Enum_Stage stage)
    {
        //存在する敵をPoolに戻す
        for(int e = 0; e < m_enemyList.Count; e++)
        {
            m_enemyList[e].ReturnPool();
        }

        //そのステージにいる敵のListをGet
        List<EnemyPlaceClass> enemyClasses = m_enemyManager.GetEnemies(stage);

        

        for(int i = 0; i < enemyClasses.Count; i++)
        {
            Enum_Enemy enemy = enemyClasses[i].Enemy;

            //そのステージの情報をGet
            EnemySpawnPosition pos = StageManager.Instance.GetEnemySpawnPosition(stage, enemy);

            //m_enemyPoolから呼ぶ
            var obj = m_enemyPoolMediation.GetPoolObject(enemy);

            var enemyObj = obj.GetComponent<EnemyController>();
            m_enemyList.Add(enemyObj);

            obj.transform.position = pos.GetSpawnPos();
        }

    }
}
