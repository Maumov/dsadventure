using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTutorial : MonoBehaviour
{
    public GameObject Content;
    public ConversationData TutorialText;
    public GameObject HelpButton;
    BaseGame gameManager;
    bool alreadyShown;
    string[] values;
    Image completeButton;

    private void Start()
    {
        gameManager = FindObjectOfType<BaseGame>();
        if(gameManager.CompleteButton != null)
            completeButton = gameManager.CompleteButton.GetComponent<Image>();
        Content.SetActive(false);
    }

    public void Show()
    {
        if(completeButton != null)
            completeButton.enabled = false;
        HelpButton.SetActive(false);
        Content.SetActive(true);
        gameManager.SetControl(false);
        gameManager.TimerState(false);
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
        HelpButton.SetActive(true);
        if(completeButton != null)
            completeButton.enabled = true;
        gameManager.TimerState(true);

        if(alreadyShown)
            return;

        gameManager.StartGame();
        alreadyShown = true;
    }
}
