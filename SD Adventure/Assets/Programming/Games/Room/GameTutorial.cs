using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorial : MonoBehaviour
{
    public GameObject Content;
    public ConversationData TutorialText;
    BaseGame gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<BaseGame>();
        Content.SetActive(false);
    }

    public void Show()
    {
        Content.SetActive(true);
        ConversationUI.ShowText(TutorialText, Finish);
    }

    void Finish()
    {
        Content.SetActive(false);
        gameManager.StartGame();
    }
}
