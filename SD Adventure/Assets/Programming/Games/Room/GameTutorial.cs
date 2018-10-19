using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorial : MonoBehaviour
{
    public GameObject Content;
    public ConversationData TutorialText;
    BaseGame gameManager;
    bool alreadyShown;

    private void Start()
    {
        gameManager = FindObjectOfType<BaseGame>();
        Content.SetActive(false);
    }

    public void Show()
    {
        Content.SetActive(true);
        gameManager.SetControl(false);
        ConversationUI.ShowText(TutorialText, Finish);
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
