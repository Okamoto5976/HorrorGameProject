using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;

    [System.Serializable]
    public class GetObjGameData
    {
        public GetObjGameData(Enum_Stage stage)
        {
            m_stage = stage;
            m_isGet = false;
        }

        private Enum_Stage m_stage;
        private bool m_isGet;

        public Enum_Stage Stage => m_stage;
        public bool IsGet => m_isGet;

        public void SetBool(bool  value)
        {
            m_isGet = value;
        }
    }

    [SerializeField] private GetObjPlacementData m_getPlacementData;

    [SerializeField] private InteractGet m_getObject;

    private List<GetObjGameData> m_getObjGameDataList = new();


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //call gameStart process
    public void Initialized()
    {
        Debug.LogWarning($"LoadProcess : GetObjectData Initialized");

        m_getObject.gameObject.SetActive( false);

        var dataList = m_getPlacementData.List;

        //ランダムに配置----------------------------

        List<Enum_Stage> stages = new();

        //設置予定をListに入れる
        for(int i = 0; i < dataList.Count; i++)
        {
            stages.Add(dataList[i].m_stage);
        }

        //選ばれたステージを入れるList
        List<Enum_Stage> candidates = new();

        //神社前は固定なので　Listに入れておく
        candidates.Add(Enum_Stage.FrontShrine);

        stages.Remove(Enum_Stage.FrontShrine);


        //3回抽選
        for(int i = 0; i < 3; i++)
        {
            int num = Random.Range(0, stages.Count);

            candidates.Add(stages[num]);

            stages.Remove(stages[num]);
        }

        //GetObjectゲームデータを登録
        foreach ( var list in candidates )
        {
            var data = new GetObjGameData(list);

            m_getObjGameDataList.Add( data );

        }
    }

    //call stagemove process
    public void SetObjectStage(Enum_Stage stage)
    {
        if (!GameManager.Instance.IsGameStart) return;

        Debug.LogWarning($"StageMove : Set GetObject in next stage : {stage}");

        foreach (var list in m_getObjGameDataList)
        {
            if(list.Stage == stage)
            {
                var pos = m_getPlacementData.List.Find(x => x != null && x.m_stage == stage).m_pos;

                m_getObject.gameObject.transform.position = pos;
                m_getObject.gameObject.SetActive(true);

                bool value = list.IsGet;

                m_getObject.Initialized(value);

                return;
            }
        }

        m_getObject.gameObject.SetActive(false);
    }

    //call Interact getObject
    public void UpdateData(Enum_Stage currentStage)
    {
        var data = m_getObjGameDataList.Find(x => x != null && x.Stage == currentStage);

        data.SetBool(true);
    }

    //save process---------------

    public GameSaveData SaveGameData(GameSaveData data)
    {
        data.getObjGameDatas = m_getObjGameDataList;

        return data;
    }

    public void SetGameData(GameSaveData data)
    {
        m_getObjGameDataList = data.getObjGameDatas;
    }
}
