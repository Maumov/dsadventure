using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeShopSelection : BaseGame
{
    [Header("Cake Shop")]
    public CakeOption[] Options;
    Vector3[] startPos;
    public float[] Scales;
    public int[] Prices;

    DragAndDrop control;

    public Collider Container;
    public Transform GoodPos;

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();
        control.OnDrop += Check;

        Randomizer.Randomize(Options);
        if(DataManager.IsHardGame)
        {
            for(int i = 0; i < Options.Length; i++)
            {
                Options[i].Text.text = "$" + Prices[i];
                Options[i].Option.transform.localScale = Vector3.one * 0.75f;
            }

        }
        else
        {
            for(int i = 0; i < Options.Length; i++)
            {
                Options[i].Option.transform.localScale = Vector3.one * Scales[i];
                Options[i].Text.text = string.Empty;
            }
        }

        startPos = new Vector3[Options.Length];
        for(int i = 0; i < startPos.Length; i++)
            startPos[i] = Options[i].Option.transform.position;

        control.Active = false;
    }

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    void Check(GameObject go)
    {
        bool correct = false;
        bool attempt = false;
        for(int i = 0; i < Options.Length; i++)
        {
            if(Container.bounds.Contains(Options[i].Option.transform.position))
            {
                if(i == Options.Length - 1)
                {
                    correct = true;
                    go.transform.position = GoodPos.position;
                }
                attempt = true;
            }
            else
                Options[i].Option.transform.position = startPos[i];
        }

        if(attempt)
        {
            control.Active = false;
            ImportantAction();

            if(DataManager.IsNAGame)
            {
                NAEnd();
                SetControl(false);
                return;
            }

            if(correct)
            {
                InGameStars.Show(LevelPos);
                if(DataManager.IsHardGame)
                    ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
                else
                    ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
            }
            else
            {
                if(DataManager.IsHardGame)
                    ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
                else
                    ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
            }

        }
    }
}
[System.Serializable]
public struct CakeOption
{
    public GameObject Option;
    public TextMesh Text;
}
