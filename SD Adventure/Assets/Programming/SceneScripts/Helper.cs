using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public float PlayerSpeed = -1;
    public PlayerPosition[] ScenePosition;
    PlayerController player;

    public void LoadScene(string scene)
    {
        SceneLoader.LoadScene(scene);
    }

    public void SetKey(string key)
    {
        DataManager.AddProgressKey(key);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        SetPlayerPosition();
    }

    void SetPlayerPosition()
    {
        for(int i = 0; i < ScenePosition.Length; i++)
        {
            if(SceneLoader.LastScene.Equals(ScenePosition[i].FromScene))
            {
                player.transform.position = ScenePosition[i].Position;
                return;
            }
        }
    }

    [System.Serializable]
    public struct PlayerPosition
    {
        public string FromScene;
        public Vector3 Position;

    }

    public string Scene;
    [ContextMenu("Add Scene Pos")]
    void AddScenePos()
    {
        for(int i = 0; i < ScenePosition.Length; i++)
        {
            if(ScenePosition[i].FromScene.Equals(Scene))
            {
                ScenePosition[i].Position = FindObjectOfType<PlayerController>().transform.position;
                return;
            }
        }

        List<PlayerPosition> p = new List<PlayerPosition>();
        p.AddRange(ScenePosition);
        PlayerPosition pos = new PlayerPosition();
        pos.FromScene = Scene;
        pos.Position = FindObjectOfType<PlayerController>().transform.position;
        p.Add(pos);
        ScenePosition = p.ToArray();
    }
}


public class Randomizer
{
    public static void Randomize<T>(T[] items)
    {
        System.Random rand = new System.Random();

        // For each spot in the array, pick
        // a random item to swap into that spot.
        for(int i = 0; i < items.Length - 1; i++)
        {
            int j = rand.Next(i, items.Length);
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
}

[System.Serializable]
public class TextKeyJson
{
    public TextKey[] Keys;
}

[System.Serializable]
public class TextKey
{
    public string Key;
    public string Value;
}