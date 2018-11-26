using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StatsHandler : MonoBehaviour
{
    const string url = "190.144.171.172/psd/upload.php";
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
    Camera cam;
    Ray ray;
    RaycastHit hit;

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
        cam = Camera.main;
    }

    public void AddAction()
    {
        stats.AddAction();
    }

    public void Send(GameStats.FinishType finishType, int acomplishment)
    {
        stats.Close(finishType, acomplishment);
        StartCoroutine(Server(TranslateJson(JsonUtility.ToJson(stats)), (sw) =>
        {
            if(!sw)
                DataManager.AddJson(TranslateJson(JsonUtility.ToJson(stats)));
        }));
    }

    public static IEnumerator Server(string json, System.Action<bool> response)
    {
        Debug.Log(json);

        WWWForm form = new WWWForm();
        form.AddField("name", "uploaded_file");
        form.AddField("data", json);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if(www.isNetworkError)
        {
            Debug.Log(www.error);
            if(response != null)
                response(false);
        }
        else
        {
            if(www.downloadHandler.text.Equals("nok"))
            {
                Debug.Log("error in server nok");
                if(response != null)
                    response(false);
            }
            else
            {
                Debug.Log("Success");
                if(response != null)
                    response(true);
            }
        }
    }

    public static IEnumerator SendRating(string json, System.Action<bool> response)
    {
        Debug.Log(json);

        WWWForm form = new WWWForm();
        form.AddField("name", "uploaded_file");
        form.AddField("data", json);
        UnityWebRequest www = UnityWebRequest.Post("http://190.144.171.172/psd/actualizarnivel.php", form);
        yield return www.SendWebRequest();

        if(www.isNetworkError)
        {
            Debug.Log(www.error);
            if(response != null)
                response(false);
        }
        else
        {
            if(www.downloadHandler.text.Equals("nok"))
            {
                Debug.Log("error in server nok");
                if(response != null)
                    response(false);
            }
            else
            {
                Debug.Log("Success");
                if(response != null)
                    response(true);
            }
        }
    }



    public void AddTouch(Vector2 pos)
    {
        if(cam != null)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 50))
            {
                stats.AddTouch(pos, hit.transform.name);
                return;
            }
        }
        stats.AddTouch(pos, "Null");
    }

    public void AddDrag(string obj, Vector2 ini, Vector2 end)
    {
        stats.AddDrag(obj, ini, end);
    }


    private void OnDestroy()
    {
        instance = null;
    }

    string TranslateJson(string json)
    {
        string str = json;

        str = str.Replace("DocVersion", "Version de Documento");
        str = str.Replace("GameVersion", "Version del Juego");
        str = str.Replace("ParameterVersion", "Version de Parametros");

        return str;
    }

}
