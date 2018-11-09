using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHandler : MonoBehaviour
{
    static StatsHandler instance;
    public static StatsHandler Instance
    {
        get
        {
            if(instance == null)
                instance = new GameObject("StatsHandler").AddComponent<StatsHandler>();
            return instance;
        }
    }

    GameStats stats;
    Vector2 pos;

    [System.NonSerialized]
    public bool initialized;

    private void Update()
    {
        if(stats != null && Input.GetMouseButtonDown(0))
        {
            pos.Set(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            AddTouch(pos);
        }
    }

    public void Create()
    {
        stats = new GameStats(SceneLoader.CurrentScene);
        initialized = true;
    }

    public void AddAction()
    {
        stats.AddAction();
    }

    public void Send(GameStats.FinishType finishType)
    {
        stats.Close(finishType);
        Debug.Log("Send to server: " + JsonUtility.ToJson(stats));
    }

    public void AddTouch(Vector2 pos)
    {
        stats.AddTouch(pos);
    }

    public void AddDrag(string obj, Vector2 ini, Vector2 end)
    {
        stats.AddDrag(obj, ini, end);
    }


    private void OnDestroy()
    {
        instance = null;
    }
}
