using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitShopPile : BaseGame
{
    [Header("Fruit Shop")]
    public LayerMask DropLayer;
    DragAndDrop control;
    Ray ray;
    RaycastHit hit;
    Vector3[] numbersPos;

    [Header("Fruit Shop Easy")]
    public GameObject EasyContent;
    public GameObject[] YellowFruit;
    public GameObject[] RedFruit;
    public GameObject[] GreenFruit;
    public GameObject[] BrownFruit;
    public Collider[] Tags;
    public GameObject NumbersParent;
    public GameObject[] Numbers;
    int yellow, red, green, brown;
    string assignedYellow, assignedRed, assignedGreen, assignedBrown;

    [Header("Fruit Shop Hard")]
    public GameObject HardContent;
    public FruitGroup[] Groups;
    public GameObject NumbersParentHard;
    public GameObject[] NumbersHard;
    int targetNumber;
    string requestedFeature;

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    public override void StartGame()
    {
        base.StartGame();
        NumbersParent.SetActive(true);
        NumbersParentHard.SetActive(true);
    }

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();

        if(DataManager.IsHardGame)
            InitHard();
        else
            InitEasy();
    }

    public void Check()
    {
        if(DataManager.IsNAGame)
        {
            NAEnd();
            SetControl(false);
            return;
        }

        if(DataManager.IsHardGame)
            CheckEasy();
        else
            CheckEasy();
    }

    void InitHard()
    {
        HardContent.SetActive(true);
        EasyContent.SetActive(false);
        CompleteButton = null;

        control.OnDrop += DropHard;

        NumbersParentHard.SetActive(false);

        numbersPos = new Vector3[NumbersHard.Length];
        for(int i = 0; i < numbersPos.Length; i++)
            numbersPos[i] = NumbersHard[i].transform.position;

        Randomizer.Randomize(Groups);
        int r;

        for(int i = 0; i < Groups.Length - 1; i++)
        {
            r = Random.Range(10, 21);
            Randomizer.Randomize(Groups[i].Fruits);
            for(int j = 0; j < Groups[i].Fruits.Length; j++)
            {
                if(j > r)
                    Groups[i].Fruits[j].SetActive(false);
            }
        }

        Randomizer.Randomize(Groups[Groups.Length - 1].Fruits);
        targetNumber = Random.Range(10, 21);
        requestedFeature = Groups[Groups.Length - 1].Feature;

        for(int i = 0; i < Groups[Groups.Length - 1].Fruits.Length; i++)
        {
            if(i > targetNumber - 1)
                Groups[Groups.Length - 1].Fruits[i].SetActive(false);
            else
                Groups[Groups.Length - 1].Fruits[i].SetActive(true);
        }

        tutorial.SetValues(new string[] { requestedFeature });
    }

    void DropHard(GameObject go)
    {
        ray = control.Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100, DropLayer.value))
        {
            ImportantAction();
            SetControl(false);
            gameSummary = "Se pidio " + targetNumber + " y marco " + go.name;
            if(DataManager.IsNAGame)
            {
                NAEnd();
                return;
            }
            if(go.name.Equals(targetNumber.ToString()))
            {
                go.transform.position = hit.transform.position + go.transform.forward * -0.01f;
                InGameStars.Show(LevelPos);
                ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
            }
            else
            {
                go.transform.position = numbersPos[go.transform.GetSiblingIndex()];
                ConversationUI.ShowText(LevelKeyName + Hard + Wrong, () => SetControl(true));
            }
        }
        else
            go.transform.position = numbersPos[go.transform.GetSiblingIndex()];
    }

    void InitEasy()
    {
        EasyContent.SetActive(true);
        HardContent.SetActive(false);

        NumbersParent.SetActive(false);

        numbersPos = new Vector3[Numbers.Length];
        for(int i = 0; i < numbersPos.Length; i++)
            numbersPos[i] = Numbers[i].transform.position;

        assignedYellow = assignedRed = assignedGreen = assignedBrown = string.Empty;

        control.OnDrop += DropEasy;

        yellow = Random.Range(4, 9);

        do
            red = Random.Range(4, 9);
        while(red == yellow);

        do
            green = Random.Range(4, 9);
        while(green == red || green == yellow);

        do
            brown = Random.Range(4, 9);
        while(brown == red || brown == yellow || brown == green);

        Randomizer.Randomize(YellowFruit);
        Randomizer.Randomize(RedFruit);
        Randomizer.Randomize(GreenFruit);
        Randomizer.Randomize(BrownFruit);

        for(int i = 0; i < 9; i++)
        {
            if(i > yellow - 1)
                YellowFruit[i].SetActive(false);

            if(i > red - 1)
                RedFruit[i].SetActive(false);

            if(i > green - 1)
                GreenFruit[i].SetActive(false);

            if(i > brown - 1)
                BrownFruit[i].SetActive(false);
        }
    }

    void DropEasy(GameObject go)
    {
        ray = control.Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100, DropLayer.value))
        {
            go.transform.position = hit.transform.position + go.transform.forward * -0.01f;
            ImportantAction();
            switch(hit.transform.name)
            {
                case "Yellow":
                    assignedYellow = go.name;
                    break;
                case "Red":
                    assignedRed = go.name;
                    break;
                case "Green":
                    assignedGreen = go.name;
                    break;
                case "Brown":
                    assignedBrown = go.name;
                    break;
            }
            hit.collider.enabled = false;
        }
        else
        {
            if(assignedYellow.Equals(go.name))
            {
                assignedYellow = string.Empty;
                Tags[0].enabled = true;
            }

            if(assignedRed.Equals(go.name))
            {
                assignedRed = string.Empty;
                Tags[1].enabled = true;
            }

            if(assignedGreen.Equals(go.name))
            {
                assignedGreen = string.Empty;
                Tags[2].enabled = true;
            }

            if(assignedBrown.Equals(go.name))
            {
                assignedBrown = string.Empty;
                Tags[3].enabled = true;
            }
            go.transform.position = numbersPos[go.transform.GetSiblingIndex()];
        }
    }

    void CheckEasy()
    {
        CompleteButton.SetActive(false);

        gameSummary = "Se pidio " + yellow + ", " + red + ", " + green + " y " + brown + ", y marco " + assignedYellow + ", " + assignedRed + ", " + assignedGreen + " y " + assignedBrown;

        if(assignedYellow.Equals(yellow.ToString()) && assignedRed.Equals(red.ToString()) && assignedGreen.Equals(green.ToString()) && assignedBrown.Equals(brown.ToString()))
        {
            InGameStars.Show(LevelPos);
            ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
        }
        else
            ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
    }



    [System.Serializable]
    public class FruitGroup
    {
        public string Feature;
        public GameObject[] Fruits;
    }
}
