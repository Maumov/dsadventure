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

    [Header("Conversations")]
    public ConversationData HardConversation;
    public ConversationData EasyConversation;

    public ConversationData GoodText;
    public ConversationData WrongText;

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();
        control.OnDrop += Check;
        control.OnDrag += Grab;

        if(DataManager.IsHardGame)
        {
            tutorial.TutorialText = HardConversation;
            for(int i = 0; i < Options.Length; i++)
                Options[i].Text.text = "" + OptionsValues[i];

        }
        else
        {
            tutorial.TutorialText = EasyConversation;
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
            go.transform.localScale = Vector3.one * 2;

            currentOrder.Add(go);
            for(int i = 0; i < currentOrder.Count; i++)
                currentOrder[i].layer = 2;
            go.layer = 10;
            Table.layer = 2;
            EnableCompleteButton();
        }
        else
        {
            for(int i = 0; i < Options.Length; i++)
            {
                if(Options[i].Option.Equals(go))
                {
                    go.transform.position = startPos[i];
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
        bool win = true;

        for(int i = 0; i < currentOrder.Count; i++)
        {
            if(!currentOrder[i].Equals(Options[i].Option))
                win = false;
        }

        if(currentOrder.Count < Options.Length)
            win = false;

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
        DataManager.AddProgressKey("CakeShopComplete", 1);
        SceneLoader.LoadScene(BaseScene);
    }
}
