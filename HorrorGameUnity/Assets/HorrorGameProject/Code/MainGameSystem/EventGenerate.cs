using UnityEngine;
using System.Collections.Generic;

public class EventGenerate : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_eventList;

    //EventManagerからIsActive（発動可能）か　IsEvent（発動済み）かのDataを貰う
    //DataをもとにSetActive　Trueにして　確率を出して待つ

}
