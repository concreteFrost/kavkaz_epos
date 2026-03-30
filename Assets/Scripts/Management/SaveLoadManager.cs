using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;


[Serializable]
public class SaveGameData
{
    public PlayerState playerState;
    public List<SaveLevelData> levelDatas;
    public string currentLevelName;

}

public static class SaveLoadManager
{
    private static string savePath => Path.Combine(Application.persistentDataPath, "save.dat");

    public static void SaveGameData(SaveGameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }

        Debug.Log("game saved");
    }

    public static SaveGameData LoadGameData()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("file not found in " + savePath);
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(savePath, FileMode.Open))
        {
            return formatter.Deserialize(stream) as SaveGameData;
        }
    }

}
