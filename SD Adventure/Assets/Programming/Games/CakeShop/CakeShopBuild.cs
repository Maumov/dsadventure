using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeShopBuild : BaseGame
{
    [Header("Cake Shop")]
    public CakeOption[] Options;
    protected Vector3[] startPos;
    public int[] OptionsValues;

    protected DragAndDrop control;
    public LayerMask CakeLayer;
    public GameObject Table;
    Ray ray;
    RaycastHit hit;
    List<GameObject> currentOrder = new List<GameObject>();

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();
        control.OnDrop += Check;
        control.OnDrag += Grab;

        if(DataManager.IsHardGame)
        {
            for(int i = 0; i < Options.Length; i++)
                Options[i].Text.text = "" + OptionsValues[i];

        }
        else
        {
            for(int i = 0; i < Options.Length; i++)
                Options[i].Text.text = "";
        }

        startPos = new Vector3[Options.Length];
        for(int i = 0; i < startPos.Length; i++)
            startPos[i] = Options[i].Option.transform.position;

        Randomizer.Randomize(startPos);

        for(int i = 0; i < startPos.Length; i++)
            Options[i].Option.transform.position = startPos[i];

        control.Active = false;
    }

    void Check(GameObject go)
    {
        ray = control.Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100, CakeLayer.value))
        {
            //hit.transform.gameObject.layer = 2;
            //go.layer = 10;
            go.transform.position = hit.transform.position;
            go.transform.position = new Vector3(go.transform.position.x, hit.collider.bounds.max.y, go.transform.position.z);
            if(!go.name.Equals("Adorno"))
                go.transform.localScale = Vector3.one * 3.5f;
            else
                go.transform.localScale = Vector3.one * 2;

            go.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = false;

            currentOrder.Add(go);
            for(int i = 0; i < currentOrder.Count; i++)
                currentOrder[i].layer = 2;
            go.layer = 10;
            Table.layer = 2;
            ImportantAction();
        }
        else
        {
            for(int i = 0; i < Options.Length; i++)
            {
                if(Options[i].Option.Equals(go))
                {
                    go.transform.position = startPos[i];
                    go.GetComponentInChildren<TextMesh>().GetComponent<MeshRenderer>().enabled = true;
                    go.layer = 0;
                    go.transform.localScale = Vector3.one;
                }
            }

            currentOrder.Remove(go);
            for(int i = 0; i < currentOrder.Count; i++)
                currentOrder[i].layer = 2;
            if(currentOrder.Count > 0)
                currentOrder[currentOrder.Count - 1].layer = 10;
            else
                Table.layer = 10;
        }
    }

    void Grab(GameObject go)
    {
        go.transform.localScale = Vector3.one;
    }

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
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

        bool win = true;

        for(int i = 0; i < currentOrder.Count; i++)
        {
            gameSummary += currentOrder[i].name + " ; ";
            if(!currentOrder[i].Equals(Options[i].Option))
                win = false;
        }

        if(currentOrder.Count < Options.Length)
            win = false;

        control.Active = false;
        if(win)
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
