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
        if(DataManager.IsHardGame)
        {
            tutorial.TutorialText = HardConversation;
            for(int i = 0; i < Options.Length; i++)
            {
                Options[i].Text.text = "$" + Prices[i];
                Options[i].Option.transform.localScale = Vector3.one * 0.5f;
            }

        }
        else
        {
            tutorial.TutorialText = EasyConversation;
            for(int i = 0; i < Options.Length; i++)
                Options[i].Option.transform.localScale = Vector3.one * Scales[i];
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

    void Check()
    {
        bool correct = false;
        bool attempt = false;
        for(int i = 0; i < Options.Length; i++)
        {
            if(Container.bounds.Contains(Options[i].Option.transform.position))
            {
                if(i == Options.Length - 1)
                    correct = true;
                attempt = true;
            }
            else
                Options[i].Option.transform.position = startPos[i];
        }

        if(attempt)
        {
            control.Active = false;
            if(correct)
                ConversationUI.ShowText(GoodText, Win);
            else
                ConversationUI.ShowText(WrongText, ResetLevel);

            EnableCompleteButton();
        }
    }

    void ResetLevel()
    {
        SceneLoader.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void Win()
    {
        DataManager.AddProgressKey("CakeShop-1", true);
        SceneLoader.LoadScene(BaseScene);
    }

}
[System.Serializable]
public struct CakeOption
{
    public GameObject Option;
    public TextMesh Text;
}
