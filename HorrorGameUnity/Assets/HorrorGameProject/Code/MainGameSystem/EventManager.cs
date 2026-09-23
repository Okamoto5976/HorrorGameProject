using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [SerializeField] private List<EventData> m_eventDataList = new();

    private List<Enum_Event> m_eventKind = new();

    //ゲーム開始時にListに出現するEventをステージごとにまとめる(仮）

    //IsActive このゲーム中　出現するか
    //IsEvent 発動済みかどうか

    //ステージ側で　確率を出して　出現可能か決めて　発動後　IsEventをTrueに

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //call stagemove process
    public void Initialized()
    {
        Debug.LogWarning($"LoadProcess : EventData Initialized");

        //ゲーム開始時に　Eventを決める

        List<Enum_Event> randomList = new();

        foreach(var e in m_eventDataList)
        {
            if (e.EventType == Enum_Event.Crow || e.EventType == Enum_Event.Train) return;

            randomList.Add(e.EventType);
        }

        //Random.Range(0f, randomList.Count);
    }

    //call stagemove process
    public void SetEventStage(Enum_Stage stage)
    {
        if (!GameManager.Instance.IsGameStart) return;

        Debug.LogWarning($"StageMove : Set Event in next stage : {stage}");

        //var list = m_eventDataList.FindAll(x => x != null && x.)
    }
}
