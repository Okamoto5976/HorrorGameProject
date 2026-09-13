using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
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
    //  Once more, stage random                                 if enemy in stage, One find not enemy stage, there stage put in list.   Seconds, random    ‚±‚ê‚È‚çŒJ‚è•Ô‚µ‚Äƒ‰ƒ“ƒ_ƒ€‚ÈŽž‚Ü‚½’Š‘I‚ð–h‚®
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
    //therds, if not find  enemy return stage //‚½‚Ô‚ñ‚È‚¢

    //public class Stage()
    //{
    //  Enum_Stage
    //  list = { (Enum_Enemy, ID).... }
    //}

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
        
        public void AddEnemy(EnemyPlaceClass enemyClass)
        {
            m_enemyList.Add(enemyClass);
        }

        //if enemy move another stage, remove enemy in list, new add in next stage
        public EnemyPlaceClass GetEnemyClass(int ID)
        {
            EnemyPlaceClass enemyClass = m_enemyList.Find(x => x != null && x.Id == ID);

            m_enemyList.Remove(enemyClass);

            return enemyClass;
        }

        public bool HasEnemy(Enum_Enemy enemy)
        {
            return m_enemyList.Exists(x => x.Enemy == enemy);
        }
    }

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

    public class EnemyInfo
    {
        public EnemyInfo(
            int id
            )
        {
            m_id = id;
        }

        private int m_id;
        private Enum_Enemy m_enemy;
        private float m_frequencyTime;

        public int ID => m_id;
        public Enum_Enemy Enemy => m_enemy;
    }

    [SerializeField] private List<EnemyData> m_enemyData = new();

    [SerializeField] private List<Enum_Stage> m_setStagePlacement = new();

    private List<StagePlacementClass> m_stagePlacementList = new();

    private void Start()
    {
        SetStagePlacementClass();
    }

    private void SetStagePlacementClass()
    {
        for(int i = 0; i < m_setStagePlacement.Count; i++)
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
    public void SetNewEnemy(Enum_Enemy enemy, EnemyPlaceClass enemyClass)
    {
        List<Enum_Stage> canStageList = GetEnemyPlacementStageData(enemy);

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
    }

    public void SetAlreadyEnemy(Enum_Enemy enemy, int id)
    {
        //once remove enemy in stage (move enemy
    }

    ////enemy exist num
    public List<Enum_Stage> GetEnemyPlacementStageData(Enum_Enemy enemy)
    {
        var canStageList = m_enemyData.Find(x => x != null && x.Enemy == enemy).CanStageList;

        List<Enum_Stage> result = new();

        foreach (Enum_Stage canStage in canStageList)
        {
            StagePlacementClass stageData = m_stagePlacementList.Find(x => x != null && x.Stage == canStage);

            if(stageData == null) continue;

            if(!stageData.HasEnemy(enemy))
            {
                result.Add(stageData.Stage);
            }
        }

        //enemy not exist stage list
        return result;
    }
}
