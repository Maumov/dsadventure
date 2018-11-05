using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGame : MonoBehaviour
{
    public string LevelKeyName;
    protected GameTutorial tutorial;
    protected bool enableControls;
    [Header("Base")]
    public GameObject CompleteButton;
    public ConversationData FirstWarning;
    public ConversationData SecondWarning;
    public string BaseScene = "Room";
    public string NextScene = "Room";
    public string[] OnCompleteKeys;

    WaitForSeconds inactivityTime = new WaitForSeconds(3000);
    public static bool Quit;

    protected const string Hard = "-HardText";
    protected const string Easy = "-EasyText";
    protected const string Wrong = "-Wrong";
    protected const string Fine = "-Fine";

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
        if(DataManager.IsHardGame)
            tutorial.TutorialText.Name = LevelKeyName + Hard;
        else
            tutorial.TutorialText.Name = LevelKeyName + Easy;

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
        SceneLoader.LoadScene(BaseScene);
    }

    public void Back()
    {
        Quit = true;
        StatsHandler.Instance.Send(GameStats.FinishType.Quit);
        SceneLoader.LoadScene(BaseScene);
    }

    public virtual void SetControl(bool sw)
    {
        enableControls = sw;
    }

    protected void Win()
    {
        for(int i = 0; i < OnCompleteKeys.Length; i++)
            DataManager.AddProgressKey(OnCompleteKeys[i], 1);

        SceneLoader.LoadScene(NextScene);
    }

    protected void ResetLevel()
    {
        StatsHandler.Instance.Send(GameStats.FinishType.Fail);
        SceneLoader.LoadScene(SceneLoader.CurrentScene);
    }
}
