using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Vector3 PlayerOffset;
    Transform player;
    Vector3 target;

    IEnumerator Start()
    {
        yield return null;
        KeyEvents[] games = FindObjectsOfType<KeyEvents>();
        Transform[] t = new Transform[games.Length];

        for(int i = 0; i < games.Length; i++)
        {
            t[games[i].transform.GetSiblingIndex() - 1] = games[i].transform;
        }

        for(int i = 0; i < t.Length; i++)
        {
            if(t[i].gameObject.activeSelf)
                target = t[i].position;
        }

        if(target == null)
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
