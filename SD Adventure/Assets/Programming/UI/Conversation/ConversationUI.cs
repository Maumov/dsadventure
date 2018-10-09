using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationUI : MonoBehaviour
{
    public GameObject Content;
    public Text Message;
    public GameObject BackButton;
    bool writing;
    bool waiting;
    WaitForSeconds charWait = new WaitForSeconds(0.05f);

    public ConversationData Test;

    void Start()
    {
        Content.SetActive(false);
        ShowText(Test);
    }

    public void Next(int direction)
    {
        if(direction == -1)
        {
            StopAllCoroutines();
            ShowText(Test);
        }
        else
        {
            if(writing)
                writing = false;
            if(waiting)
                waiting = false;
        }
    }

    public void ShowText(ConversationData msg, System.Action onFinish = null)
    {
        Content.SetActive(true);
        StartCoroutine(WriteText(msg, onFinish));
    }

    IEnumerator WriteText(ConversationData msg, System.Action onFinish)
    {
        string displayText;

        for(int i = 0; i < msg.Pages.Length; i++)
        {
            BackButton.SetActive(i != 0);
            writing = true;
            displayText = string.Empty;
            for(int j = 0; j < msg.Pages[i].Length && writing; j++)
            {
                displayText = string.Concat(displayText, msg.Pages[i][j]);
                Message.text = displayText;
                yield return charWait;
            }
            writing = false;
            Message.text = msg.Pages[i];

            waiting = true;
            while(waiting)
                yield return null;
        }

        Content.SetActive(false);
        yield return charWait;
        if(onFinish != null)
            onFinish();
    }


}

[System.Serializable]
public class ConversationData
{
    public string Name;
    public string[] Pages;
}
