﻿using System.Collections;
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

    public static bool IsHardGame
    {
        get
        {
            Check();
            return Data.GameFiles[selectedFile].HardGame;
        }
    }

    public static FileData GetSelectedFile()
    {
        Check();
        return Data.GameFiles[selectedFile];
    }

    public static void AddProgressKey(string key, int value = 0)
    {
        Check();
        if(!Data.GameFiles[selectedFile].CheckKey(key))
            Data.GameFiles[selectedFile].AddKey(key, value);
        Save();
    }

    public static bool CheckProgressKey(string key)
    {
        Check();
        return Data.GameFiles[selectedFile].CheckKey(key);
    }

    public static bool ProgressKeyValue(string key, out int value)
    {
        Check();
        return Data.GameFiles[selectedFile].GetKeyValue(key, out value);
    }

    public static void SetAsHardGame()
    {
        Check();
        //bool total = false, current = false;
        //string[] gamesKeys = new string[] { "CarGame", "CubesGame", "DicesGame", "ToysGame", "BasketGame" };
        //for(int i = 0; i < gamesKeys.Length; i++)
        //{
        //    ProgressKeyValue(gamesKeys[i], out current);

        //    if(i == 0)
        //        total = current;
        //    else
        //        total = current && total;
        //}
        //if(total)
        //    Data.GameFiles[selectedFile].HardGame = true;

        Data.GameFiles[selectedFile].HardGame = false;
        Save();
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
    public bool HardGame;
    public List<ProgressKey> ProgressKeys = new List<ProgressKey>();

    public bool CheckKey(string k)
    {
        for(int i = 0; i < ProgressKeys.Count; i++)
        {
            if(ProgressKeys[i].Key.Equals(k))
                return true;
        }

        return false;
    }

    public void AddKey(string k, int v)
    {
        ProgressKeys.Add(new ProgressKey(k, v));
    }

    public bool GetKeyValue(string key, out int v)
    {
        for(int i = 0; i < ProgressKeys.Count; i++)
        {
            if(ProgressKeys[i].Key.Equals(key))
            {
                v = ProgressKeys[i].Value;
                return true;
            }
        }

        v = -1;
        return false;
    }
}

[System.Serializable]
public class ProgressKey
{
    public string Key;
    public int Value;

    public ProgressKey(string k, int v)
    {
        Key = k;
        Value = v;
    }
}