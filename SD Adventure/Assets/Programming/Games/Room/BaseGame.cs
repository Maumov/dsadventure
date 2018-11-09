using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGame : MonoBehaviour
{
    public string LevelKeyName;
    public bool OverrideTutorial = true;
    protected GameTutorial tutorial;
    protected bool enableControls;
    [Header("Base")]
    public GameObject CompleteButton;
    public string BaseScene = "Room";
    public string NextScene = "Room";
    public string[] OnCompleteKeys;

    float timeLimit1 = 180;
    float timeLimit2 = 60;
    float startTime;
    int clues;

    public static bool Quit;

    protected const string Hard = "-HardText";
    protected const string Easy = "-EasyText";
    protected const string Wrong = "-Wrong";
    protected const string Fine = "-Fine";
    protected const string Warning = "-Warning";
    protected const string Clue = "-Clue";
    bool firstAction;

    const float inactivityLimit = 30;
    float currentInactivitry;
    bool counting;

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
        if(OverrideTutorial)
        {
            if(DataManager.IsHardGame)
                tutorial.TutorialText.Name = LevelKeyName + Hard;
            else
                tutorial.TutorialText.Name = LevelKeyName + Easy;
        }

        yield return null;
        tutorial.Show();
    }

    public virtual void StartGame()
    {
        enableControls = true;
        StatsHandler.Instance.Create();
        counting = true;
        StartCoroutine(InactivityCounter());
        startTime = Time.time;
    }

    protected void ImportantAction()
    {
        if(CompleteButton != null && !CompleteButton.activeInHierarchy)
            CompleteButton.SetActive(true);

        StatsHandler.Instance.AddAction();
        StopAllCoroutines();
        counting = true;
        StartCoroutine(InactivityCounter());
        firstAction = true;
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
        currentInactivitry = 0;
        while(currentInactivitry < inactivityLimit)
        {
            if(counting)
                currentInactivitry += Time.deltaTime;
            yield return null;
        }

        SetControl(false);
        if(firstAction)
        {
            SetCompleteButton(false);

            if(OverrideTutorial)
            {
                ConversationUI.ShowText(LevelKeyName + Clue + 1, Restore);
                clues++;
            }
            else
                ConversationUI.ShowText(LevelKeyName + Warning + 1, Restore);
        }
        else
            ConversationUI.ShowText(LevelKeyName + Warning + 1, () => SetControl(true));

        currentInactivitry = 0;
        while(currentInactivitry < inactivityLimit)
        {
            if(counting)
                currentInactivitry += Time.deltaTime;
            yield return null;
        }

        SetControl(false);
        if(firstAction)
        {
            SetCompleteButton(false);

            if(OverrideTutorial)
            {
                ConversationUI.ShowText(LevelKeyName + Clue + 2, Restore);
                clues++;
            }
            else
                ConversationUI.ShowText(LevelKeyName + Warning + 2, Restore);
        }
        else
            ConversationUI.ShowText(LevelKeyName + Warning + 2, () => SetControl(true));

        currentInactivitry = 0;
        while(currentInactivitry < inactivityLimit)
        {
            if(counting)
                currentInactivitry += Time.deltaTime;
            yield return null;
        }
        Quit = true;
        StatsHandler.Instance.Send(GameStats.FinishType.Afk);
        SceneLoader.LoadScene(BaseScene);
    }

    void Restore()
    {
        SetControl(true);
        SetCompleteButton(true);
    }

    void SetCompleteButton(bool sw)
    {
        if(CompleteButton != null)
            CompleteButton.SetActive(sw);
    }

    public void Back()
    {
        SetControl(false);
        counting = false;
        ConfirmationPopUp.GetConfirmation("¿Quieres salir?", (sw) =>
        {
            if(sw)
            {
                Quit = true;
                if(StatsHandler.Instance.initialized)
                    StatsHandler.Instance.Send(GameStats.FinishType.Quit);
                SceneLoader.LoadScene(BaseScene);
            }
            else
            {
                SetControl(true);
                counting = true;
            }
        });
    }

    public virtual void SetControl(bool sw)
    {
        enableControls = sw;
    }

    protected void Win()
    {
        if(DataManager.GetSelectedFile().GameDifficult != 0)
        {
            if(Time.time - startTime > timeLimit2 || clues > 1)
            {
                Debug.Log("Fin no exitoso");
                ResetLevel();
                return;
            }
            else if(Time.time - startTime < timeLimit1 && clues == 0)
            {
                Debug.Log("Fin exitoso 1");
            }
            else
                Debug.Log("Fin exitoso 2");
        }

        StatsHandler.Instance.Send(GameStats.FinishType.Complete);
        for(int i = 0; i < OnCompleteKeys.Length; i++)
            DataManager.AddProgressKey(OnCompleteKeys[i], 1);

        SceneLoader.LoadScene(NextScene);
    }

    protected void ResetLevel()
    {
        StatsHandler.Instance.Send(GameStats.FinishType.Fail);
        SceneLoader.LoadScene(SceneLoader.CurrentScene);
    }


    public void TimerState(bool sw)
    {
        counting = sw;
    }
}