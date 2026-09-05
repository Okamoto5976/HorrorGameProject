using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [SerializeField] private StageData m_stageData;

    private void Start()
    {
        MainManager.Instance.SetCurrentStage(m_stageData.MyStage);
    }

}
