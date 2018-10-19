using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DataManager
{
    static SaveData Data;
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

    public static void DeleteFile(int fileId)
    {
        Check();
        Data.GameFiles.RemoveAt(fileId);
        Save();
    }

    static void Load()
    {
        string dataAsJson = PlayerPrefs.GetString("Data");

        if(string.IsNullOrEmpty(dataAsJson))
            Data = new SaveData();
        else
            Data = JsonUtility.FromJson<SaveData>(dataAsJson);
    }

    public static void Save()
    {
        if(Data == null)
            Load();
        PlayerPrefs.SetString("Data", JsonUtility.ToJson(Data));
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
