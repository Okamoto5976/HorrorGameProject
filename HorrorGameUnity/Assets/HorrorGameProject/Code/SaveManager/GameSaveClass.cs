using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class GameSaveData
{
    public int collect;
    public float time;

    public Enum_Stage stage;

    //player data
    public Vector3 pos;

    //EventがActiveかIsEvent（発動済み）か

    //敵の動き
    //List <enemy , position>　Listに今いる敵を出し、どこのステージにいるか
    //敵の生成は時間ごと　後半に出てくるものがいる

    public List<PlacementManager.GetObjGameData> getObjGameDatas;

    //Event Data
}
public class GameSaveClass
{
    public void SaveGameData(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        string path = Application.persistentDataPath + "/gameSave.json";

        File.WriteAllText(path, json);
    }

    public GameSaveData LoadGameData()
    {
        string path = Application.persistentDataPath + "/gameSave.json";

        if(!File.Exists(path))
        {
            return null;
        }

        string json = File.ReadAllText(path);

        GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

        return data;
    }

    public void DeleteGameSaveData()
    {
        string path = Application.persistentDataPath + "/gameSave.json";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public bool CheckSaveData()
    {
        string path = Application.persistentDataPath + "/gameSave.json";

        if (File.Exists(path))
        {
            return true;
        }

        return false;
    }
}
