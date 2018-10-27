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

    WaitForSeconds inactivityTime = new WaitForSeconds(30);
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
        StatsHandler.Instance.Create();
        StartCoroutine(InactivityCounter());
    }

    protected void ImportantAction()
    {
        if(CompleteButton != null && !CompleteButton.activeInHierarchy)
            CompleteButton.SetActive(true);

        StatsHandler.Instance.AddAction();
        StopAllCoroutines();
        StartCoroutine(InactivityCounter());
    }

    public virtual void Complete()
    {
        CompleteValidations();
        StatsHandler.Instance.Send(GameStats.FinishType.Complete);
        SceneLoader.LoadScene(BaseScene);
    }

    protected virtual void CompleteValidations() { }


    IEnumerator InactivityCounter()
    {
        yield return inactivityTime;
        SetControl(false);
        ConversationUI.ShowText(FirstWarning, () => SetControl(true));

        yield return inactivityTime;
        SetControl(false);
        ConversationUI.ShowText(SecondWarning, () => SetControl(true));

        yield return inactivityTime;
        Quit = true;
        StatsHandler.Instance.Send(GameStats.FinishType.Afk);
        SceneLoader.LoadScene("Room");
    }

    public void Back()
    {
        Quit = true;
        StatsHandler.Instance.Send(GameStats.FinishType.Quit);
        SceneLoader.LoadScene("Room");
    }

    public virtual void SetControl(bool sw)
    {
        enableControls = sw;
    }
}
