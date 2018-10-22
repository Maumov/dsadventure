using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeShopBaking : BaseGame
{

    [Header("Cake Shop")]
    public GameObject[] Options;
    DragAndDrop control;
    Vector3[] startPos;

    [Header("Conversations")]
    public ConversationData HardConversation;
    public ConversationData EasyConversation;

    public ConversationData GoodText;
    public ConversationData WrongText;

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();
        control.OnDrop += Check;

        startPos = new Vector3[Options.Length];
        for(int i = 0; i < startPos.Length; i++)
            startPos[i] = Options[i].transform.position;
    }


    void Check()
    {
        for(int i = 0; i < Options.Length; i++)
            Options[i].transform.position = startPos[i];
    }

}
