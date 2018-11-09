using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTalk : MonoBehaviour
{
    public Transform NpcFront;
    PlayerController player;
    public string ComingFrom;

    public void Auto()
    {
        StartCoroutine(Talk());
    }

    IEnumerator Talk()
    {
        if(SceneLoader.LastScene.Equals(ComingFrom))
        {
            player = FindObjectOfType<PlayerController>();
            yield return null;
            player.transform.position = NpcFront.position;
            player.transform.rotation = NpcFront.rotation;
            yield return new WaitForSeconds(0.5f);
            player.ForceInteraction(GetComponent<Interaction>());

        }
    }
}
