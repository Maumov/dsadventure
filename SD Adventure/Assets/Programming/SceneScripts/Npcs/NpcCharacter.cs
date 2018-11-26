using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCharacter : InteractionObject
{
    public string NpcName;
    PlayerController controller;
    static NpcDatabase database;
    Animator anim;
    Vector3 pos;

    void Start()
    {
        Animator[] a = GetComponentsInChildren<Animator>();
        for(int i = 0; i < a.Length; i++)
        {
            if(a[i].runtimeAnimatorController.name.Equals("Avatar"))
                anim = a[i];
        }
        anim.transform.localScale = Vector3.one * 1.2f;
        anim.transform.localEulerAngles = new Vector3(0, 0, 0);
        controller = FindObjectOfType<PlayerController>();
        if(database == null)
            database = FindObjectOfType<Helper>().NpcTexts;
    }

    public override void Action()
    {
        base.Action();
        controller.ControlState = false;
        ConversationUI.ShowText(database.GetConversation(NpcName), () => controller.ControlState = true);
        anim.CrossFade("Talk", 0.25f);

        pos = controller.transform.position;
        pos.y = anim.transform.position.y;
        anim.transform.LookAt(pos);
    }
}
