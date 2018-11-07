using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainNpc : MonoBehaviour
{
    public string Conversation;
    string toScene;

    PlayerController controller;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
    }

    public void Talk(string scene)
    {
        toScene = scene;
        controller.ControlState = false;
        ConversationUI.ShowText(Conversation, LoadLevel);
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
