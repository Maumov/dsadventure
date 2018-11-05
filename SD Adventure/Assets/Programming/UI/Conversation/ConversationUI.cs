﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationUI : MonoBehaviour
{
    public GameObject Content;
    public Text Message;
    public GameObject BackButton;
    public Sprite Arrow;
    public Sprite FinalText;
    public Image NextButton;
    bool writing;
    bool waiting;
    bool back;
    WaitForSeconds charWait = new WaitForSeconds(0.05f);

    static ConversationUI instance;
    public TextAsset ConversationKeys;
    ConversationData[] allConversations;

    const string playerName = "<nombre>";

    void Awake()
    {
        instance = this;
        Content.SetActive(false);
        allConversations = JsonUtility.FromJson<ConversationKeyJson>(ConversationKeys.text).Conversations;
    }

    public void Next(int direction)
    {
        if(direction == -1)
        {
            back = true;
            writing = false;
        }
        else
        {
            if(writing)
                writing = false;
            if(waiting)
                waiting = false;
        }
    }

    public static void ShowText(ConversationData msg, System.Action onFinish = null)
    {
        instance.Content.SetActive(true);
        instance.StartCoroutine(instance.WriteText(instance.GetConversation(msg), onFinish));
    }

    public static void ShowText(string msg, System.Action onFinish = null)
    {
        instance.Content.SetActive(true);
        instance.StartCoroutine(instance.WriteText(instance.GetConversation(msg), onFinish));
    }

    IEnumerator WriteText(ConversationData msg, System.Action onFinish)
    {
        string displayText;
        NextButton.sprite = Arrow;
        
        for(int i = 0; i < msg.Pages.Length; i++)
        {
            BackButton.SetActive(i != 0);
            writing = true;
            displayText = string.Empty;

            msg.Pages[i] = msg.Pages[i].Replace(playerName, DataManager.GetSelectedFile().FileName);

            for(int j = 0; j < msg.Pages[i].Length && writing; j++)
            {
                displayText = string.Concat(displayText, msg.Pages[i][j]);
                Message.text = displayText;
                yield return charWait;
            }

            if(back)
            {
                i -= 2;
                back = false;
                continue;
            }
            writing = false;
            Message.text = msg.Pages[i];

            if(i == msg.Pages.Length - 1)
                NextButton.sprite = FinalText;
            else
                NextButton.sprite = Arrow;

            waiting = true;
            while(waiting)
            {
                if(back)
                {
                    i--;
                    back = false;
                    waiting = false;
                }
                yield return null;
            }
        }

        Content.SetActive(false);
        yield return charWait;
        if(onFinish != null)
            onFinish();
    }

    ConversationData GetConversation(ConversationData cd)
    {
        for(int i = 0; i < allConversations.Length; i++)
            if(allConversations[i].Name.Equals(cd.Name) && allConversations[i].Pages != null && !string.IsNullOrEmpty(allConversations[i].Pages[0]))
                return allConversations[i];

        cd.Pages = new string[] { cd.Name };
        return cd;
    }

    ConversationData GetConversation(string cd)
    {
        for(int i = 0; i < allConversations.Length; i++)
            if(allConversations[i].Name.Equals(cd) && allConversations[i].Pages != null && !string.IsNullOrEmpty(allConversations[i].Pages[0]))
                return allConversations[i];

        ConversationData c = new ConversationData();
        c.Pages = new string[] { cd};
        return c;
    }
}

[System.Serializable]
public class ConversationData
{
    public string Name;
    public string[] Pages;
}

[System.Serializable]
public class ConversationKeyJson
{
    public ConversationData[] Conversations;
}