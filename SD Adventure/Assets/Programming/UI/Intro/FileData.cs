using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DataManager
{
    static SaveData Data;
    const string fileName = "/data.json";
    static int selectedFile;

    static void Check()
    {
        if(Data == null)
            Load();
    }

    public static FileData GetSelectedFile()
    {
        Check();
        return Data.GameFiles[selectedFile];
    }

    public static void SetSelectedFile(int i)
    {
        selectedFile = i;
    }

    public static FileData[] GetAllFiles()
    {
        Check();
        return Data.GameFiles.ToArray();
    }

    public static void AddFile(FileData file)
    {
        Check();
        Data.GameFiles.Add(file);
        Save();
    }

    static void Load()
    {
        string filePath = Application.persistentDataPath + fileName;

        if(File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            Data = JsonUtility.FromJson<SaveData>(dataAsJson);
        }
        else
            Data = new SaveData();
    }

    public static void Save()
    {
        string filePath = Application.persistentDataPath + fileName;
        if(Data == null)
            Load();
        File.WriteAllText(filePath, JsonUtility.ToJson(Data));
    }

    public static void RestoreData()
    {
        Data = new SaveData();
        Save();
    }
}

[System.Serializable]
public class SaveData
{
    public List<FileData> GameFiles;

    public SaveData()
    {
        GameFiles = new List<FileData>();
    }
}

[System.Serializable]
public class FileData
{
    public string FileName;
    public int AvatarId;
    public string LastScene;
}
