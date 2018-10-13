using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGame : MonoBehaviour
{
    GameTutorial tutorial;
    protected bool enableControls;
    [Header("Base")]
    public GameObject CompleteButton;
    public ConversationData FirstWarning;
    public ConversationData SecondWarning;

    WaitForSeconds inactivityTime = new WaitForSeconds(10);

    public virtual void Start()
    {
        tutorial = FindObjectOfType<GameTutorial>();
        CompleteButton.SetActive(false);
        StartCoroutine(ShowDelay());
    }

    IEnumerator ShowDelay()
    {
        yield return null;
        tutorial.Show();
    }

    public void StartGame()
    {
        enableControls = true;
        StartCoroutine(InactivityCounter());
    }

    protected void EnableCompleteButton()
    {
        CompleteButton.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(InactivityCounter());
    }

    public virtual void Complete()
    {

    }


    IEnumerator InactivityCounter()
    {
        yield return inactivityTime;
        ConversationUI.ShowText(FirstWarning);

        yield return inactivityTime;
        ConversationUI.ShowText(SecondWarning);

        yield return inactivityTime;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Room");
        Debug.Log("AFK");
    }
}
