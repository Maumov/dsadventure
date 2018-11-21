using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendNpc : InteractionObject
{
    PlayerController player;
    public static string CurrentConversation;
    static NpcDatabase database;

    public bool FollowPlayer;
    float speed;
    Vector3 followPos;
    Vector3 followDirection;
    CharacterController controller;

    IEnumerator Start()
    {
        player = FindObjectOfType<PlayerController>();
        controller = GetComponent<CharacterController>();
        if(database == null)
            database = FindObjectOfType<Helper>().NpcTexts;

        if(FollowPlayer)
        {
            transform.position = player.transform.position + Vector3.forward * 2;
            player.Friend = this;
            yield return null;
            speed = player.MovementSpeed;
        }
    }

    public override void Action()
    {
        base.Action();
        player.ControlState = false;
        if(!string.IsNullOrEmpty(CurrentConversation))
            ConversationUI.ShowText(CurrentConversation, () => player.ControlState = true);
        else
            ConversationUI.ShowText(database.GetConversation(string.Empty), () => player.ControlState = true);
    }

    void Update()
    {
        if(!FollowPlayer)
            return;

        followPos = player.transform.position;
        followPos.y = transform.position.y;
        if(Vector3.SqrMagnitude(followPos - transform.position) > 1)
        {
            transform.LookAt(followPos);
            followDirection = (followPos - transform.position).normalized;
            controller.Move((followDirection * speed + Vector3.up * -5) * Time.deltaTime);
        }

    }

    public void Hide()
    {
        Icon.enabled = false;
    }

    public void Show()
    {
        Icon.enabled = true;
    }

}
