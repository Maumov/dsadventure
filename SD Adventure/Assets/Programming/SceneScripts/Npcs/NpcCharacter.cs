using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCharacter : InteractionObject
{
    public string NpcName;
    PlayerController controller;
    static NpcDatabase database;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
        if(database == null)
            database = FindObjectOfType<Helper>().NpcTexts;
    }

    public override void Action()
    {
        base.Action();
        controller.ControlState = false;
        ConversationUI.ShowText(database.GetConversation(NpcName), () => controller.ControlState = true);
    }
}
