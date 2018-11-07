using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorial : MonoBehaviour
{
    public GameObject Content;
    public ConversationData TutorialText;
    BaseGame gameManager;
    bool alreadyShown;
    string[] values;

    private void Start()
    {
        gameManager = FindObjectOfType<BaseGame>();
        Content.SetActive(false);
    }

    public void Show()
    {
        Content.SetActive(true);
        gameManager.SetControl(false);
        ConversationUI.ShowText(TutorialText, Finish, values);
    }

    public void SetValues(string[] v)
    {
        values = v;
    }

    void Finish()
    {
        Content.SetActive(false);
        gameManager.SetControl(true);
        if(alreadyShown)
            return;

        gameManager.StartGame();
        alreadyShown = true;
    }
}
