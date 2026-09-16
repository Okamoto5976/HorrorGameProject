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

public class OptionSaveClass
{
    //public string m_dir => Application.persistentDataPath;
    //public string m_optionFile => Path.Combine(m_dir, "/optionSave.json");

    public void SaveOptionData(OptionSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        string path = Application.persistentDataPath + "/audioSave.json";

        File.WriteAllText(path, json);
    }

    public OptionSaveData LoadOptionData()
    {
        string path = Application.persistentDataPath + "/audioSave.json";

        if (!File.Exists(path))
        {
            return null;
        }

        string json = File.ReadAllText(path);

        OptionSaveData data = JsonUtility.FromJson<OptionSaveData>(json);

        return data;
    }

    public void DeleteOptionSaveData()
    {
        string path = Application.persistentDataPath + "/audioSave.json";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
