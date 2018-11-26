using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainNpc : MonoBehaviour
{
    public string Conversation;
    string toScene;

    PlayerController controller;
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
        anim.transform.localEulerAngles = new Vector3(0, 180, 0);
        controller = FindObjectOfType<PlayerController>();
    }

    public void Talk(string scene)
    {
        toScene = scene;
        controller.ControlState = false;
        ConversationUI.ShowText(Conversation, LoadLevel);
        anim.CrossFade("Talk", 0.25f);

        pos = controller.transform.position;
        pos.y = anim.transform.position.y;
        anim.transform.LookAt(pos);
    }

    void LoadLevel()
    {
        ConfirmationPopUp.GetConfirmation("¿Quieres ayudar?", Response);
    }

    void Response(bool sw)
    {
        if(sw)
            SceneLoader.LoadScene(toScene);
        else
            controller.ControlState = true;
    }
}
