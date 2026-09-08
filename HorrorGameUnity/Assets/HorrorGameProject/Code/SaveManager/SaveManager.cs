using System.IO;
using UnityEngine;

[System.Serializable]
public class OptionSaveData
{
    public float m_masterVolume;
    public float m_bgmVolume;
    public float m_seVolume;
    public float m_voiceVolume;

    public bool m_goreCheck;
}

[System.Serializable]
public class MainGameSaveData
{
    public int m_collect;
    public float m_time;

    //EventがActiveかIsPlay（発動済み）か

    //敵の動き
    //List <enemy , position>　Listに今いる敵を出し、どこのステージにいるか
    //敵の生成は時間ごと　後半に出てくるものがいる
}

public class SaveManager : MonoBehaviour
{
    public string m_dir => Application.persistentDataPath;
    public string m_optionFile => Path.Combine(m_dir, "optionSave.json");

    public void OnOptionSave()
    {

    }
}
