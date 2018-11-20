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

    public static void AddJson(string json)
    {
        Check();
        GetSelectedFile().PendingJsonFiles.Add(json);
        Debug.Log(GetSelectedFile().PendingJsonFiles.Count);
        Save();
    }

    public static bool IsHardGame
    {
        get
        {
            Check();
            return Data.GameFiles[selectedFile].GameDifficult == 2;
        }
    }

    public static bool IsNAGame
    {
        get
        {
            Check();
            return Data.GameFiles[selectedFile].GameDifficult == 0;
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
        Debug.Log(JsonUtility.ToJson(new EvaluationJson(i.ToString())));
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
    public string Age;
    public string FileId;
    public int AvatarId;
    public string LastScene;
    public int GameDifficult = -1;
    public List<ProgressKey> ProgressKeys = new List<ProgressKey>();
    public List<string> PendingJsonFiles = new List<string>();

    public FileData(string fileName, string age, int avatarId)
    {
        FileName = fileName;
        Age = age;
        FileId = (fileName + DataHelper.GetTime().ToString()).GetHashCode().ToString();

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

    #endregion

    #region Constructors
    public GameStats(string name)
    {
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
    public class PlayerInfo
    {
        public string ID;
        public string Nivel;
        public string Nombre;
        public string Edad;

        public GameSessionData[] GameSessions;

        public PlayerInfo()
        {
            ID = DataManager.GetSelectedFile().FileId;
            Nivel = DataManager.GetSelectedFile().GameDifficult.ToString();
            Nombre = DataManager.GetSelectedFile().FileName;
            Edad = DataManager.GetSelectedFile().Age;

            GameSessions = new GameSessionData[] { new GameSessionData() };
        }
    }

    [System.Serializable]
    public class GameSessionData
    {
        public string TimeStamp;
        public MinigameSessionData[] MiniGameSessions;

        public GameSessionData()
        {
            TimeStamp = DataHelper.GetTime().ToString();
            MiniGameSessions = new MinigameSessionData[] { new MinigameSessionData() };
        }
    }

    [System.Serializable]
    public class MinigameSessionData
    {
        public string ID;
        public string TimeStamp;
        public ActivitySessionData[] ActivitySessions;

        public MinigameSessionData()
        {
            ID = "0";
            TimeStamp = DataHelper.GetTime().ToString();
            ActivitySessions = new ActivitySessionData[] { new ActivitySessionData() };
        }
    }

    [System.Serializable]
    public class ActivitySessionData
    {
        public string ID;
        [System.NonSerialized]
        public string GameName;
        public string TimeStampStart;
        public string TimeStampEnd;
        public string TimeToFirstEvent;
        public int LevelOfAccomplishment;
        [System.NonSerialized]
        public int ActionsAmount;
        [System.NonSerialized]
        public FinishType EndBy;

        public List<ActionEventData> ActionEvents;

        public ActivitySessionData()
        {
            ID = "0";
            GameName = "";// name;
            TimeStampStart = DataHelper.GetTime().ToString();
            TimeToFirstEvent = "";
            ActionsAmount = 0;
            EndBy = FinishType.None;
            ActionEvents = new List<ActionEventData>();
        }
    }

    [System.Serializable]
    public class ActionEventData
    {
        public string TimeStamp;
        public string type;
        public string CoordinatesStart;
        public string CoordinatesEnd;
        public string ObjectInteractedID;

        public ActionEventData(Vector2 pos, string obj)//touch
        {
            TimeStamp = DataHelper.GetTime().ToString();
            type = "touch";
            CoordinatesStart = pos.ToString();
            ObjectInteractedID = obj;
        }

        public ActionEventData(Vector2 ini, Vector2 end, string obj)//touch
        {
            TimeStamp = DataHelper.GetTime().ToString();
            type = "DAndD";
            CoordinatesStart = ini.ToString();
            CoordinatesEnd = end.ToString();
            ObjectInteractedID = obj;
        }
    }
    #endregion

    #region Methods
    public void AddAction()
    {
        if(Players[0].GameSessions[0].MiniGameSessions[0].ActivitySessions[0].ActionsAmount == 0)
            Players[0].GameSessions[0].MiniGameSessions[0].ActivitySessions[0].TimeToFirstEvent = DataHelper.GetTime().ToString();
        Players[0].GameSessions[0].MiniGameSessions[0].ActivitySessions[0].ActionsAmount++;
    }

    public void Close(FinishType finishType, int acomplishment)
    {
        Players[0].GameSessions[0].MiniGameSessions[0].ActivitySessions[0].TimeStampEnd = DataHelper.GetTime().ToString();
        Players[0].GameSessions[0].MiniGameSessions[0].ActivitySessions[0].LevelOfAccomplishment = acomplishment;
        Players[0].GameSessions[0].MiniGameSessions[0].ActivitySessions[0].EndBy = finishType;
    }

    public void AddTouch(Vector2 pos, string obj)
    {
        Players[0].GameSessions[0].MiniGameSessions[0].ActivitySessions[0].ActionEvents.Add(new ActionEventData(pos, obj));
    }

    public void AddDrag(string obj, Vector2 ini, Vector2 end)
    {
        Players[0].GameSessions[0].MiniGameSessions[0].ActivitySessions[0].ActionEvents.Add(new ActionEventData(new Vector2(ini.x / Screen.width, ini.y / Screen.height), new Vector2(end.x / Screen.width, end.y / Screen.height), obj));
    }
    #endregion
}

public class DataHelper
{
    public static int GetTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
    }
}

[System.Serializable]
public class EvaluationJson
{
    public string Id;
    public string Time;
    public string Level;

    public EvaluationJson(string e)
    {
        Id = DataManager.GetSelectedFile().FileId;
        Time = DataHelper.GetTime().ToString();
        Level = e;
    }
}