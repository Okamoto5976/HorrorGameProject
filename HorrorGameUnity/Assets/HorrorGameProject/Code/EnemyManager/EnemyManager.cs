using UnityEngine;
using System.Collections.Generic;

//そのステージごとの敵のList
public class StagePlacementClass
{
    public StagePlacementClass(
        Enum_Stage stage
        )
    {
        m_stage = stage;
    }

    private Enum_Stage m_stage;

    private List<EnemyPlaceClass> m_enemyList = new();

    public Enum_Stage Stage => m_stage;
    public List<EnemyPlaceClass> EnemyList => m_enemyList;

    /// <summary>
    /// 敵を追加する処理
    /// </summary>
    public void AddEnemy(EnemyPlaceClass enemyClass)
    {
        m_enemyList.Add(enemyClass);
    }

    /// <summary>
    /// 敵のIDを渡すと探す処理
    /// </summary>
    //if enemy move another stage, remove enemy in list, new add in next stage
    public EnemyPlaceClass GetEnemyByID(int ID)
    {
        EnemyPlaceClass enemyClass = m_enemyList.Find(x => x != null && x.Id == ID);

        if (enemyClass == null)
        {
            //Debug.Log($"{m_stage}: not enemy by id");
            return null;
        }
        //Debug.Log($"{m_stage}: find enemy by id");

        m_enemyList.Remove(enemyClass);

        return enemyClass;
    }

    /// <summary>
    /// 敵のタイプで同じ種類の敵がいるかどうか返す処理
    /// </summary>
    public bool HasEnemy(Enum_Enemy enemy)
    {
        return m_enemyList.Exists(x => x.Enemy == enemy);
    }
}


//敵の種類とID
public class EnemyPlaceClass
{
    public EnemyPlaceClass(
        Enum_Enemy enemy,
        int id
        )
    {
        m_enemy = enemy;
        m_id = id;
    }

    private Enum_Enemy m_enemy;
    private int m_id;

    public Enum_Enemy Enemy => m_enemy;
    public int Id => m_id;
}

public class EnemyManager : MonoBehaviour
{
    //敵個体ごとの情報
    public class EnemyInfo
    {
        public EnemyInfo(
            int id,
            Enum_Enemy enemy,
            float frequencyTime
            )
        {
            m_id = id;
            m_enemy = enemy;
            m_frequencyTime = frequencyTime;

            m_currentTime = m_frequencyTime;
        }

        private int m_id;
        private Enum_Enemy m_enemy;
        private float m_frequencyTime;

        public float m_currentTime;

        public int ID => m_id;
        public Enum_Enemy Enemy => m_enemy;

        public void SetCurrentTime()
        {
            m_currentTime = m_frequencyTime;
        }
    }

    [SerializeField] private List<EnemyData> m_enemyData = new();

    [SerializeField] private List<Enum_Stage> m_setStagePlacement = new();

    [SerializeField] private List<EnemyInfo> m_enemyInfoList = new();

    private List<StagePlacementClass> m_stagePlacementList = new();

    //Component
    //[SerializeField] private EnemyGenerater m_enemyGenerate;

    private void Start()
    {
        SetStagePlacementClass();
    }

    
    /// <summary>
    /// 敵が出現するStageListごとにステージクラスを作る
    /// </summary>
    private void SetStagePlacementClass()
    {
        for (int i = 0; i < m_setStagePlacement.Count; i++)
        {
            Enum_Stage stage = m_setStagePlacement[i];

            var stageClass = new StagePlacementClass(stage);

            m_stagePlacementList.Add(stageClass);
        }
    }

    //look maxExistNum or need ID
    //new enemy class
    //EnemyPlaceClass enemyClass = new
    //(
    //    enemy,
    //    id
    //);

    private int m_nextID = 1;


    //call GameStart
    [ContextMenu("Test Add Enemy")]
    public void TestAddEnemy()
    {
        var enemy = Enum_Enemy.Ghost;

        //make info
        var info = MakeEnemyInfo(enemy, m_nextID);
        
        m_enemyInfoList.Add(info);

        //make EnemyPlaceClass
        EnemyPlaceClass enemyClass = new(enemy, m_nextID);

        AddEnemy(enemyClass);

        m_nextID++;
    }

    private void Update()
    {
        for(int i = 0; i < m_enemyInfoList.Count; i++)
        {
            m_enemyInfoList[i].m_currentTime -= Time.deltaTime;

            if (m_enemyInfoList[i].m_currentTime <= 0)
            {
                //敵を移動させる処理
                MoveEnemy(m_enemyInfoList[i]);

                //Debug.LogWarning($"{m_enemyInfoList[i].ID}:{m_enemyInfoList[i].Enemy} -> Move Enemy");

                //敵のTimeをリセット
                m_enemyInfoList[i].SetCurrentTime();
            }
        }
    }

    /// <summary>
    /// 新しくEnemyInfo（敵の情報）を作る処理
    /// </summary>
    /// <returns>出来たEnemyInfoを返す</returns>
    private EnemyInfo MakeEnemyInfo(Enum_Enemy enemy, int id)
    {
        EnemyData data = m_enemyData.Find(x => x != null && x.Enemy == enemy);

        EnemyInfo info = new EnemyInfo(id, data.Enemy, data.FrequencyTime);

        return info;
    }

    /// <summary>
    /// 新しく敵を配置する処理
    /// </summary>
    public void AddEnemy(EnemyPlaceClass enemyClass)
    {
        List<Enum_Stage> canStageList = GetEnemyPlacementStageData(enemyClass.Enemy);

        if (canStageList.Count <= 0)
        {
            Debug.LogWarning("Not find can stage");
            return;
        }

        //select random from canStageList----------------
        int num = Random.Range(0, canStageList.Count);

        Enum_Stage stage = canStageList[num];

        StagePlacementClass stagePlacement = m_stagePlacementList.Find(x => x != null && x.Stage == stage);
        //-----------------------------------------------

        //add Enemy in stage to select
        stagePlacement.AddEnemy(enemyClass);

        Debug.Log($"{enemyClass.Id}:{enemyClass.Enemy} -> move {stage}");
    }

    /// <summary>
    /// 既存の敵がステージを動く処理
    /// </summary>
    private void MoveEnemy(EnemyInfo info)
    {
        //まずランダムに次に行く場所を探す
        List<Enum_Stage> canStageList = GetEnemyPlacementStageData(info.Enemy);

        if (canStageList.Count <= 0)
        {
            Debug.LogWarning("Not find can stage");
            return;
        }
        int num = Random.Range(0, canStageList.Count);

        Enum_Stage stage = canStageList[num];

        StagePlacementClass stagePlacement = m_stagePlacementList.Find(x => x != null && x.Stage == stage);
        //-------------------------------------



        EnemyPlaceClass enemyClass = null;

        //同じIdの敵を探す
        for(int i = 0; i < m_stagePlacementList.Count; i++)
        {
            enemyClass = m_stagePlacementList[i].GetEnemyByID(info.ID);

            if(enemyClass != null)
            {
                //Debug.LogWarning("Find enemy by id");

                break;
            }
        }

        if(enemyClass == null)
        {
            Debug.LogWarning("Not find enemy by id");
            return;
        }

        stagePlacement.AddEnemy(enemyClass);
        Debug.Log($"{enemyClass.Id}:{enemyClass.Enemy} -> move {stage}");

        //StagePlacementClassを抜き　新しく移動させる（Add)

    }

    /// <summary>
    /// 可動域ステージから現在敵がいないStageListを送る
    /// </summary>
    private List<Enum_Stage> GetEnemyPlacementStageData(Enum_Enemy enemy)
    {
        var canStageList = m_enemyData.Find(x => x != null && x.Enemy == enemy).CanStageList;

        List<Enum_Stage> result = new();

        foreach (Enum_Stage canStage in canStageList)
        {
            StagePlacementClass stageData = m_stagePlacementList.Find(x => x != null && x.Stage == canStage);

            if (stageData == null) continue;

            if (!stageData.HasEnemy(enemy))
            {
                result.Add(stageData.Stage);
            }
        }

        //enemy not exist stage list
        return result;
    }

    public List<EnemyPlaceClass> GetEnemies(Enum_Stage stage)
    {
        //return m_stagePlacementList.Find(x => x != null && x.Stage == stage).EnemyList;

        StagePlacementClass placement = m_stagePlacementList.Find(x => x != null && x.Stage == stage);

        if (placement == null)
        {
            return null;
        }

        return placement.EnemyList;
    }

    //Player move Stage => Stage in Enemy Get info  class stage or enemyList
    //Update Frame...  enemy in time if 0    Get CanStageList in enemyData    Remove and new Add
    //canStageList random...  but Stage have limit 

    //-------------------------------------------------------------------------
    //All Enemy Class ==> find enemy list in map
    //Ghost
    //{
    //  Class placement (stage or enemy)    placement.count == maxExist
    //
    //  when want to place enemy in stage if this stage already exist enemy, not input in stage of enemy
    //  Once more, stage random                                 if enemy in stage, One find not enemy stage, there stage put in list.   Seconds, random    これなら繰り返してランダムな時また抽選を防ぐ
    //
    //}
    //
    //
    //
    //--------------------------------------------------------------------------
    //
    //All Stage Class
    //1Stage
    //{
    //  Ghost or ID
    //  Teketeke or ID
    //}

    //Exsit Enemy in Map
    //Ghost
    //{
    //  ID, frequencyTime, 
    //}

    //if move stage, onece find not enemy in stage   put in list    
    //seconds, random
    //therds, if not find  enemy return stage //たぶんない

    //public class Stage()
    //{
    //  Enum_Stage
    //  list = { (Enum_Enemy, ID).... }
    //}



}
