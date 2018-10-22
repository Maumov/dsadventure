using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeShopBaking : BaseGame
{
    [Header("Cake Shop")]
    public CakeOption[] Options;
    DragAndDrop control;
    public Vector3[] Scales;
    public int[] Weights;

    Vector3[] startPos;

    public Collider[] Containers;

    [Header("Conversations")]
    public ConversationData HardConversation;
    public ConversationData EasyConversation;

    public ConversationData GoodText;
    public ConversationData WrongText;

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();
        control.OnDrop += Check;

        Randomizer.Randomize(Options);
        Debug.Log(DataManager.IsHardGame);
        if(DataManager.IsHardGame)
        {
            tutorial.TutorialText = HardConversation;
            for(int i = 0; i < Options.Length; i++)
            {
                Options[i].Text.text = Weights[i] + "";
                Options[i].Option.transform.localScale = Scales[1];
            }

        }
        else
        {
            tutorial.TutorialText = EasyConversation;
            for(int i = 0; i < Options.Length; i++)
            {
                Options[i].Option.transform.localScale = Scales[i];
                Options[i].Text.text = string.Empty;
            }
        }


        startPos = new Vector3[Options.Length];
        for(int i = 0; i < startPos.Length; i++)
            startPos[i] = Options[i].Option.transform.position;
    }

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    void Check(GameObject go)
    {
        bool inPos;

        for(int i = 0; i < Options.Length; i++)
        {
            inPos = false;
            for(int j = 0; j < Containers.Length; j++)
            {
                if(Containers[j].enabled && Containers[j].bounds.Contains(Options[i].Option.transform.position))
                {
                    Options[i].Option.transform.position = Containers[j].transform.position;
                    Containers[j].enabled = false;
                    inPos = true;
                    EnableCompleteButton();
                    break;
                }
                if(Containers[j].transform.position == Options[i].Option.transform.position)
                    inPos = true;
            }
            if(!inPos)
                Options[i].Option.transform.position = startPos[i];
        }

        bool empty;

        for(int i = 0; i < Containers.Length; i++)
        {
            empty = true;
            for(int j = 0; j < Options.Length; j++)
            {
                if(Containers[i].bounds.Contains(Options[j].Option.transform.position))
                {
                    empty = false;
                    break;
                }
            }
            Containers[i].enabled = empty;
        }
    }

    public override void Complete()
    {
        CompleteButton.SetActive(false);
        bool win = true;
        for(int i = 0; i < Containers.Length; i++)
        {
            if(!Containers[i].bounds.Contains(Options[i].Option.transform.position))
                win = false;
        }

        control.Active = false;
        if(win)
            ConversationUI.ShowText(GoodText, Win);
        else
            ConversationUI.ShowText(WrongText, ResetLevel);
    }


    void ResetLevel()
    {
        SceneLoader.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void Win()
    {
        DataManager.AddProgressKey("CakeShop-2", true);
        DataManager.AddProgressKey("PetShopOpen", true);
        SceneLoader.LoadScene(BaseScene);
    }

}
