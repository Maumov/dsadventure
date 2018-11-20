using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendNpc : InteractionObject
{
    PlayerController controller;
    public static string CurrentConversation;
    bool followPlayer;

    void Start()
    {
        controller = FindObjectOfType<PlayerController>();
    }

    public override void Action()
    {
        base.Action();
        controller.ControlState = false;
        ConversationUI.ShowText(CurrentConversation, () => controller.ControlState = true);
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
