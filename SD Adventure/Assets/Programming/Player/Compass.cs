using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Vector3 PlayerOffset;
    Transform player;
    Vector3 target;
    KeyEvents[] games;

    void Awake()
    {
        games = FindObjectsOfType<KeyEvents>();
    }

    IEnumerator Start()
    {
        Transform[] t = new Transform[games.Length];

        for(int i = 0; i < games.Length; i++)
            t[games[i].transform.GetSiblingIndex() - 1] = games[i].transform;

        yield return null;

        for(int i = 0; i < t.Length; i++)
            if(t[i].gameObject.activeSelf)
                target = t[i].position;

        if(target == Vector3.zero)
            gameObject.SetActive(false);
        else
            player = FindObjectOfType<PlayerController>().transform;
    }

    void LateUpdate()
    {
        if(target == Vector3.zero)
            return;

        transform.position = player.position + PlayerOffset;
        target.y = transform.position.y;
        transform.LookAt(target);
    }

}
