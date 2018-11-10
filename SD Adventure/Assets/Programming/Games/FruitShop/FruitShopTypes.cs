using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitShopTypes : BaseGame
{
    [Header("Fruit Shop")]
    public LayerMask DropLayer;
    DragAndDrop control;
    Ray ray;
    RaycastHit hit;
    int targetNumber = 5;

    [Header("Fruit Shop Easy")]
    public GameObject EasyContent;
    public TextMesh RequiredFruits;
    public Collider FruitContainer;
    public GameObject[] EasyFruits;
    Vector3 easyStartPos;


    [Header("Fruit Shop Hard")]
    public GameObject HardContent;
    public GameObject[] YellowFruit;
    public GameObject[] RedFruit;
    public GameObject[] GreenFruit;
    public GameObject NumbersParent;
    public GameObject[] Numbers;
    public Collider[] Tags;
    Vector3[] numbersPos;
    int yellow, red, green;
    string asignedYellow, asignedRed, asignedGreen;

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    public override void StartGame()
    {
        base.StartGame();
        NumbersParent.SetActive(true);
    }

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();
        NumbersParent.SetActive(false);

        if(DataManager.IsHardGame)
            InitHard();
        else
            InitEasy();
    }

    public void Check()
    {
        if(DataManager.IsHardGame)
            CheckHard();
        else
            CheckEasy();
    }

    void InitEasy()
    {
        HardContent.SetActive(false);
        EasyContent.SetActive(true);

        control.OnDrag += EasyDrag;
        control.OnDrop += EasyDrop;

        targetNumber = Random.Range(4, 9);
        RequiredFruits.text = targetNumber.ToString();

        tutorial.SetValues(new string[] { targetNumber.ToString() });
    }

    void EasyDrag(GameObject go)
    {
        easyStartPos = go.transform.position;
    }

    void EasyDrop(GameObject go)
    {
        ray = control.Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100, DropLayer.value))
        {
            go.transform.position = hit.point;
            if(hit.transform.name.Equals("Target"))
                ImportantAction();
        }
        else
            go.transform.position = easyStartPos;
    }

    void CheckEasy()
    {
        int lemons = 0;
        SetControl(false);
        CompleteButton.SetActive(false);
        for(int i = 0; i < EasyFruits.Length; i++)
        {
            if(FruitContainer.bounds.Contains(EasyFruits[i].transform.position))
            {
                if(i < 11)
                {
                    ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
                    return;
                }
                else
                    lemons++;
            }
        }
        if(lemons != targetNumber)
        {
            ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
            return;
        }

        ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
    }

    void InitHard()
    {

        asignedYellow = asignedRed = asignedGreen = string.Empty;

        EasyContent.SetActive(false);
        HardContent.SetActive(true);

        control.OnDrop += DropHard;

        numbersPos = new Vector3[Numbers.Length];
        for(int i = 0; i < numbersPos.Length; i++)
            numbersPos[i] = Numbers[i].transform.position;

        yellow = Random.Range(10, 21);
        do
            red = Random.Range(10, 21);
        while(red == yellow);

        do
            green = Random.Range(10, 21);
        while(green == red || green == yellow);


        Randomizer.Randomize(YellowFruit);
        Randomizer.Randomize(RedFruit);
        Randomizer.Randomize(GreenFruit);

        for(int i = 0; i < 20; i++)
        {
            if(i > yellow - 1)
                YellowFruit[i].SetActive(false);

            if(i > red - 1)
                RedFruit[i].SetActive(false);

            if(i > green - 1)
                GreenFruit[i].SetActive(false);
        }
    }

    void DropHard(GameObject go)
    {
        ray = control.Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100, DropLayer.value))
        {
            go.transform.position = hit.transform.position + go.transform.forward * -0.01f;
            ImportantAction();
            switch(hit.transform.name)
            {
                case "Yellow":
                    asignedYellow = go.name;
                    break;
                case "Red":
                    asignedRed = go.name;
                    break;
                case "Green":
                    asignedGreen = go.name;
                    break;
            }
            hit.collider.enabled = false;
        }
        else
        {
            if(asignedYellow.Equals(go.name))
            {
                asignedYellow = string.Empty;
                Tags[0].enabled = true;
            }

            if(asignedRed.Equals(go.name))
            {
                asignedRed = string.Empty;
                Tags[1].enabled = true;
            }

            if(asignedGreen.Equals(go.name))
            {
                asignedGreen = string.Empty;
                Tags[2].enabled = true;
            }
            go.transform.position = numbersPos[go.transform.GetSiblingIndex()];
        }
    }

    void CheckHard()
    {
        CompleteButton.SetActive(false);
        SetControl(false);
        if(asignedYellow.Equals(yellow.ToString()) && asignedRed.Equals(red.ToString()) && asignedGreen.Equals(green.ToString()))
            ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
        else
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
    }
}
