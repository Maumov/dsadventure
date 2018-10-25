using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGame : MonoBehaviour
{
    protected GameTutorial tutorial;
    protected bool enableControls;
    [Header("Base")]
    public GameObject CompleteButton;
    public ConversationData FirstWarning;
    public ConversationData SecondWarning;
    public string BaseScene = "Room";

    WaitForSeconds inactivityTime = new WaitForSeconds(5);
    public static bool Quit;

    protected virtual void Start()
    {
        tutorial = FindObjectOfType<GameTutorial>();
        if(CompleteButton != null)
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
        if(CompleteButton != null)
            CompleteButton.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(InactivityCounter());
    }

    public virtual void Complete()
    {
        CompleteValidations();
        SceneLoader.LoadScene(BaseScene);
    }

    protected virtual void CompleteValidations() { }


    IEnumerator InactivityCounter()
    {
        yield return inactivityTime;
        SetControl(false);
        ConversationUI.ShowText(FirstWarning, ()=> SetControl(true));

        yield return inactivityTime;
        SetControl(false);
        ConversationUI.ShowText(SecondWarning, () => SetControl(true));

        yield return inactivityTime;
        Quit = true;
        SceneLoader.LoadScene("Room");
    }

    public void Back()
    {
        Quit = true;
        SceneLoader.LoadScene("Room");
    }

    public virtual void SetControl(bool sw)
    {
        enableControls = sw;
    }
}
