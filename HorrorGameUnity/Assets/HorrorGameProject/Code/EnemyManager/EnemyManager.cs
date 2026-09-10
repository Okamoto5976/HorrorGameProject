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
    //therds, if not find  enemy return stage 

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

        private List<EnemyInfo> m_enemyList = new();

        public Enum_Stage Stage => m_stage;
        public List<EnemyInfo> EnemyList => m_enemyList;
        
        //public void 
    }

    public class EnemyInfo
    { 
        public EnemyInfo(
            )
        {

        }

        private Enum_Enemy m_enemy;
        private int m_id;
    }

    [SerializeField] private List<EnemyData> m_enemyData = new();

    private List<StagePlacementClass> m_stagePlacementList = new();


}
