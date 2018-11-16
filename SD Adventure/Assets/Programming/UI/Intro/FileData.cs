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

    public static bool IsHardGame
    {
        get
        {
            Check();
            return Data.GameFiles[selectedFile].GameDifficult == 2;
        }
    }

    public static string LastScene
    {
        get
        {
            if(string.IsNullOrEmpty(Data.GameFiles[selectedFile].LastScene))
                return "Room";
            return Data.GameFiles[selectedFile].LastScene;
        }
        set
        {
            Data.GameFiles[selectedFile].LastScene = value;
            Save();
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

    public static void SetGameDifficult(int i)
    {
        Check();
        Data.GameFiles[selectedFile].GameDifficult = i;
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

    public static void Load()
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
    public string FileId;
    public int AvatarId;
    public string LastScene;
    public int GameDifficult = -1;
    public List<ProgressKey> ProgressKeys = new List<ProgressKey>();

    public FileData(string fileName, int avatarId)
    {
        FileName = fileName;

        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        FileId = (fileName + cur_time.ToString()).GetHashCode().ToString();

        AvatarId = avatarId;
        LastScene = "";
        GameDifficult = -1;
        ProgressKeys = new List<ProgressKey>();
    }

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

[System.Serializable]
public class GameStats
{

    #region Variables
    public string DocVersion = "0.1";
    public string GameVersion = "0.1";
    public string ParameterVersion = "0.1";

    public PlayerInfo[] Players;

    public string GameName;
    public float FirstActionTime;
    public float GameTime;
    public int ActionsAmount;
    public FinishType EndBy;

    public List<Vector2> TouchCount;
    public List<DragInfo> DragCount;

    [System.NonSerialized]
    float startTime;
    [System.NonSerialized]
    DragInfo currentDrag;

    #endregion

    #region Constructors
    public GameStats(string name)
    {
        GameName = name;
        FirstActionTime = -1;
        GameTime = -1;
        ActionsAmount = 0;
        EndBy = FinishType.None;
        TouchCount = new List<Vector2>();
        DragCount = new List<DragInfo>();

        startTime = Time.time;
        currentDrag = new DragInfo("empty");

        Players = new PlayerInfo[] { new PlayerInfo() };
    }

    [System.Serializable]
    public enum FinishType
    {
        None = 0,
        Complete,
        Quit,
        Afk,
        Fail
    }

    [System.Serializable]
    public struct DragInfo
    {
        public string ObjectName;
        public Vector2 Ini;
        public Vector2 End;

        public DragInfo(string str)
        {
            ObjectName = str;
            Ini = new Vector2(0, 0);
            End = new Vector2(0, 0);
        }

        public DragInfo(string obj, Vector2 i, Vector2 e)
        {
            ObjectName = obj;
            Ini = i;
            End = e;
        }
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public string ID;
        public string Nivel;
        public string Nombre;
        public string Edad;

        public PlayerInfo()
        {
            ID = DataManager.GetSelectedFile().FileId;
            Nivel = DataManager.GetSelectedFile().GameDifficult.ToString();
            Nombre = DataManager.GetSelectedFile().FileName;
            Edad = "";
        }
    }


    #endregion

    #region Methods
    public void AddAction()
    {
        if(ActionsAmount == 0)
            FirstActionTime = Time.time - startTime;
        ActionsAmount++;
    }

    public void Close(FinishType finishType)
    {
        GameTime = Time.time - startTime;
        EndBy = finishType;
    }

    public void AddTouch(Vector2 pos)
    {
        TouchCount.Add(pos);
    }

    public void AddDrag(string obj, Vector2 ini, Vector2 end)
    {
        currentDrag.ObjectName = obj;
        currentDrag.Ini.Set(ini.x / Screen.width, ini.y / Screen.height);
        currentDrag.End.Set(end.x / Screen.width, end.y / Screen.height);

        DragCount.Add(currentDrag);
    }
    #endregion
}

//System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
//int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
//miniGameSession = new MiniGameSession(MiniGameID, cur_time.ToString ());