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

    protected override void Initialize()
    {
        
        control = FindObjectOfType<DragAndDrop>();
        control.OnDrop += Check;

        Randomizer.Randomize(Options);

        for(int i = 0; i < Options.Length; i++)
            Options[i].Option.name = "Molde " + i;

        if(DataManager.IsHardGame)
        {
            for(int i = 0; i < Options.Length; i++)
            {
                Options[i].Text.text = Weights[i] + "";
                //Options[i].Option.transform.localScale = Scales[1];
            }

        }
        else
        {
            for(int i = 0; i < Options.Length; i++)
            {
                Options[i].Option.transform.localScale = Scales[i];
                Options[i].Text.text = string.Empty;
            }
        }

        startPos = new Vector3[Options.Length];
        for (int i = 0; i < startPos.Length; i++) {
            startPos [i] = Options [i].Option.transform.position;
        }

        Summary ();

    }

    protected override void Summary() { 
        for(int i = 0; i < Options.Length; i++){
            Vector2 pos = Camera.main.ViewportToScreenPoint (Options [i].Option.transform.position);
            gameObjets += "ObjectID:" + i+1 +":" + pos.x+ " , " + pos.y;
        }

        for(int i = 0; i < Containers.Length; i++){
            Vector2 pos = Camera.main.ViewportToScreenPoint (Containers [i].transform.position);
            gameSockets += "SocketID" + i+1 +":" + pos.x+ " , " + pos.y;
        }
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
                    ImportantAction();
                    break;
                }
                if(Containers[j].transform.position == Options[i].Option.transform.position)
                    inPos = true;
            }
            if(!inPos)
                Options[i].Option.transform.position = startPos[i];
        }

        for(int i = 0; i < Containers.Length; i++)
        {
            Containers[i].enabled = true;
            for(int j = 0; j < Options.Length; j++)
            {
                if(Containers[i].bounds.Contains(Options[j].Option.transform.position))
                    Containers[i].enabled = false;
            }
        }
    }

    public override void Complete()
    {
        CompleteButton.SetActive(false);
        if(DataManager.IsNAGame)
        {
            NAEnd();
            SetControl(false);
            return;
        }

        for(int i = 0; i < Containers.Length; i++)
            Containers[i].enabled = true;


        bool win = true;
        for(int i = 0; i < Containers.Length; i++)
        {
            if(!Containers[i].bounds.Contains(Options[i].Option.transform.position))
                win = false;
        }

        if(!win)
        {
            win = true;
            System.Array.Reverse(Containers);
            for(int i = 0; i < Containers.Length; i++)
            {
                if(!Containers[i].bounds.Contains(Options[i].Option.transform.position))
                    win = false;
            }
        }

        System.Array.Reverse(Containers);
        for(int i = 0; i < Containers.Length; i++)
        {
            for(int j = 0; j < Options.Length; j++)
            {
                if(Containers[i].bounds.Contains(Options[j].Option.transform.position))
                {
                    gameSummary += "Horno " + (i + 1) + " tiene " + " molde " + (j + 1) + ";";
                }
            }
        }

        control.Active = false;
        if(win)
        {
            for(int i = 0; i < Options.Length; i++)
                Options[i].Option.transform.GetChild(2).gameObject.SetActive(true);

            InGameStars.Show(LevelPos);
            if(DataManager.IsHardGame)
                ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
            else
                ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
        }
        else
        {
            for(int i = 0; i < Options.Length; i++)
                Options[i].Option.transform.GetChild(1).gameObject.SetActive(true);

            if(DataManager.IsHardGame)
                ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
            else
                ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
        }
    }
}
