using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void save(List<LevelData> levels, List<ScoreData> scores) // Modify the save method
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerV.data";

        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(levels, scores); // Modify the instantiation of PlayerData
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData load()
    {
        string path = Application.persistentDataPath + "/playerV.data";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            //Debug.LogError("save file not found");
            return null;
        }
    }

    public static bool verifPath()
    {
        string path = Application.persistentDataPath + "/playerV.data";
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
