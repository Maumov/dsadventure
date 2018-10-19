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

    WaitForSeconds inactivityTime = new WaitForSeconds(100);

    protected virtual void Start()
    {
        tutorial = FindObjectOfType<GameTutorial>();
        CompleteButton.SetActive(false);
        StartCoroutine(ShowDelay());
        Initialize();
    }

    protected virtual void Initialize() { }

    IEnumerator ShowDelay()
    {
        yield return null;
        tutorial.Show();
    }

    public virtual void StartGame()
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
        CompleteValidations();
        SceneLoader.LoadScene("Room");
    }

    protected virtual void CompleteValidations() { }


    IEnumerator InactivityCounter()
    {
        yield return inactivityTime;
        ConversationUI.ShowText(FirstWarning);

        yield return inactivityTime;
        ConversationUI.ShowText(SecondWarning);

        yield return inactivityTime;
        SceneLoader.LoadScene("Room");
        Debug.Log("AFK");
    }

    public void Back()
    {
        SceneLoader.LoadScene("Room");
    }

    public virtual void SetControl(bool sw)
    {
        enableControls = sw;
    }
}
