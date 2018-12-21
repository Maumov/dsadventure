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
    public float FinishDelay;
    WaitForSeconds finishWait;

    float timeLimit1 = 240;
    float timeLimit2 = 120;
    float startTime;
    int clues;

    protected int acomplishmentLevel;

    public static bool Quit;

    protected const string Hard = "-HardText";
    protected const string Easy = "-EasyText";
    protected const string Wrong = "-Wrong";
    protected const string Fine = "-Fine";
    protected const string Warning = "-Warning";
    protected const string Clue = "-Clue";
    bool firstAction;

    const float inactivityLimit = 30;
    float currentInactivity;
    bool counting;


    public int LevelPos;

    public static string CurrentSceneId;
    public static string CurrentMinigameId;

    [Header("Server info")]
    public string SceneId;
    public string MinigameId;
    protected string gameSummary;
    protected string gameObjets;
    protected string gameSockets;

    protected virtual void Start()
    {
        CurrentSceneId = SceneId;
        CurrentMinigameId = MinigameId;

        finishWait = new WaitForSeconds(FinishDelay);
        tutorial = FindObjectOfType<GameTutorial>();
        if(CompleteButton != null)
            CompleteButton.SetActive(false);
        StartCoroutine(ShowDelay());
        Initialize();

        if(LevelKeyName.Contains("1"))
            LevelPos = 1;
        else if(LevelKeyName.Contains("2"))
            LevelPos = 2;
        else
            LevelPos = 3;
    }

    protected virtual void Initialize() { }

    protected virtual void Summary() { 
        Debug.LogError ("Summary Not Done Here!");
    }

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
        StatsHandler.Instance.Send(GameStats.FinishType.Complete, acomplishmentLevel, gameSummary, gameObjets, gameSockets);
        StartCoroutine(CompleteDelay());
    }

    IEnumerator CompleteDelay()
    {
        yield return finishWait;
        SceneLoader.LoadScene(BaseScene);
    }

    protected virtual void CompleteValidations() { }


    IEnumerator InactivityCounter()
    {
        currentInactivity = 0;
        while(currentInactivity < inactivityLimit)
        {
            if(counting)
                currentInactivity += Time.deltaTime;
            yield return null;
        }

        SetControl(false);
        if(firstAction)
        {
            SetCompleteButton(false);

            if(OverrideTutorial)
            {
                ConversationUI.ShowText(LevelKeyName + Clue + "-" + 1, Restore);
                clues++;
            }
            else
                ConversationUI.ShowText(LevelKeyName + Warning + "-" + 1, Restore);
        }
        else
            ConversationUI.ShowText(LevelKeyName + Warning + "-" + 1, () => SetControl(true));

        currentInactivity = 0;
        while(currentInactivity < inactivityLimit)
        {
            if(counting)
                currentInactivity += Time.deltaTime;
            yield return null;
        }

        SetControl(false);
        if(firstAction)
        {
            SetCompleteButton(false);

            if(OverrideTutorial)
            {
                ConversationUI.ShowText(LevelKeyName + Clue + "-" + 2, Restore);
                clues++;
            }
            else
                ConversationUI.ShowText(LevelKeyName + Warning + "-" + 2, Restore);
        }
        else
            ConversationUI.ShowText(LevelKeyName + Warning + "-" + 2, () => SetControl(true));

        currentInactivity = 0;
        while(currentInactivity < inactivityLimit)
        {
            if(counting)
                currentInactivity += Time.deltaTime;
            yield return null;
        }
        Quit = true;
        StatsHandler.Instance.Send(GameStats.FinishType.Afk, -1, "AFK Player", gameObjets, gameSockets);
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
                    StatsHandler.Instance.Send(GameStats.FinishType.Quit, -1, "Player Quits", gameObjets, gameSockets);
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
        int v = 0;
        if(DataManager.GetSelectedFile().GameDifficult != 0)
        {
            if(Time.time - startTime > timeLimit2 || clues > 1)
            {
                Debug.Log("Fin no exitoso");
                ConversationUI.ShowText("DoItFaster", ResetLevel);
                //ResetLevel();
                return;
            }
            else if(Time.time - startTime < timeLimit1 && clues == 0)
            {
                Debug.Log("Fin exitoso 1");
                v = 1;
            }
            else
            {
                Debug.Log("Fin exitoso 2");
                v = 2;
            }
        }

        StatsHandler.Instance.Send(GameStats.FinishType.Complete, v, gameSummary, gameObjets, gameSockets);
        for(int i = 0; i < OnCompleteKeys.Length; i++)
            DataManager.AddProgressKey(OnCompleteKeys[i], v);

        SceneLoader.LoadScene(NextScene);
    }

    protected void ResetLevel()
    {
        StatsHandler.Instance.Send(GameStats.FinishType.Fail, -1, gameSummary, gameObjets, gameSockets);
        SceneLoader.LoadScene(SceneLoader.CurrentScene);
    }


    public void TimerState(bool sw)
    {
        counting = sw;
    }

    protected void NAEnd()
    {
        TimerState(false);
        InGameStars.Show(LevelPos);
        ConversationUI.ShowText("GenericaNAText", () =>
        {
            StatsHandler.Instance.Send(GameStats.FinishType.Complete, -1, "N/A Player", gameObjets, gameSockets);
            for(int i = 0; i < OnCompleteKeys.Length; i++)
                DataManager.AddProgressKey(OnCompleteKeys[i], -1);

            SceneLoader.LoadScene(NextScene);
        });


    }

    WaitForSeconds checkStarTime = new WaitForSeconds(1);
    IEnumerator CheckStars()
    {
        while(!InGameStars.cancelStar)
        {
            if(DataManager.GetSelectedFile().GameDifficult != 0)
            {
                if(Time.time - startTime > timeLimit2 || clues > 1)
                    InGameStars.cancelStar = true;
            }
            yield return checkStarTime;
        }
    }

	public static Vector2 ScreenCoordinates(Camera cam, Vector3 pos){
		//return new Vector2(pos.x / Screen.width, pos.y / Screen.height);
		Vector2 p = cam.WorldToViewportPoint (pos);
		return new Vector2(p.x , p.y);
	}
}