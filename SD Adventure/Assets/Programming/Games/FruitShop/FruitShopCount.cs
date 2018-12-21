using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitShopCount : BaseGame
{
    public Camera gameCamEasy;
    public Camera gameCamHard;
    [Header("Fruit Shop")]
    public LayerMask DropLayer;
    DragAndDrop control;
    Ray ray;
    RaycastHit hit;
    int targetNumber = 5;

    [Header("Fruit Shop Easy")]
    public GameObject EasyContent;
    public GameObject[] Numbers;
    public GameObject[] EasyFruits;
    public Transform GoodPosition;
    Vector3[] startPos;


    [Header("Fruit Shop Hard")]
    public GameObject HardContent;
    public TextMesh RequiredFruits;
    public Collider FruitContainer;
    public GameObject[] HardFruits;
    Vector3 hardStartPos;

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    public override void StartGame()
    {
        base.StartGame();

        for(int i = 0; i < Numbers.Length; i++)
            Numbers[i].SetActive(true);
    }

    protected override void Initialize()
    {
        if(DataManager.IsHardGame)
            InitHard();
        else
            InitEasy();

        SetControl(false);

        Summary ();
    }
    protected override void Summary(){
        if (DataManager.IsHardGame) {
            for (int i = 0; i < HardFruits.Length; i++) {
                Vector2 pos = ScreenCoordinates(gameCamHard,HardFruits [i].transform.position);
                gameObjets += "" + i + "," + pos.x + "," + pos.y+";";
            }

            Vector2 p = ScreenCoordinates(gameCamHard,FruitContainer.transform.position);
            gameSockets += "0," + p.x + "," + p.y+";";

        }else{
            for(int i = 0; i < Numbers.Length; i++){
                Vector2 pos = ScreenCoordinates(gameCamEasy,Numbers [i].transform.position);
                gameObjets += ""+ i +"," + pos.x+ "," + pos.y+","+Numbers[i].name+";";
            }
            Vector2 p = ScreenCoordinates(gameCamEasy,GoodPosition.transform.position);
            gameSockets += "0," + p.x + "," + p.y+";";

        }

    }


    void InitHard()
    {
        HardContent.SetActive(true);
        EasyContent.SetActive(false);

        control = FindObjectOfType<DragAndDrop>();

        control.OnDrag += HardDrag;
        control.OnDrop += HardDrop;

        targetNumber = Random.Range(10, 21);
        tutorial.SetValues(new string[] { targetNumber.ToString() });
        RequiredFruits.text = targetNumber.ToString();
    }

    void HardDrag(GameObject go)
    {
        hardStartPos = go.transform.position;
    }

    void HardDrop(GameObject go)
    {
        ray = control.Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100, DropLayer.value))
        {
            go.transform.position = hit.point;
            if(hit.transform.name.Equals("Target"))
                ImportantAction();
        }
        else
            go.transform.position = hardStartPos;
    }

    public void CheckHard()
    {
        SetControl(false);
        CompleteButton.SetActive(false);
        gameSummary = "0,"+targetNumber+",";
        for(int i = 0; i < HardFruits.Length; i++)
        {
            if(FruitContainer.bounds.Contains(HardFruits[i].transform.position))
            {
                gameSummary += "," + i ;
            }
        }
        gameSummary += ";";
        if(DataManager.IsNAGame)
        {
            InGameStars.Show(LevelPos);
            NAEnd();
            return;
        }

        int lemons = 0;
        for(int i = 0; i < HardFruits.Length; i++)
        {
            if(FruitContainer.bounds.Contains(HardFruits[i].transform.position))
            {
                if(i < 9)
                {
                   
                    ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
                    return;
                }
                else
                    lemons++;
            }
        }
       
        if(lemons != targetNumber)
        {
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
            return;
        }
        InGameStars.Show(LevelPos);
        ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);

    }

    void InitEasy()
    {
        HardContent.SetActive(false);
        EasyContent.SetActive(true);

        control = FindObjectOfType<DragAndDrop>();

        CompleteButton = null;
        startPos = new Vector3[Numbers.Length];
        for(int i = 0; i < startPos.Length; i++)
        {
            startPos[i] = Numbers[i].transform.position;
            Numbers[i].SetActive(false);
        }

        control.OnDrop += EasyDrop;

        targetNumber = Random.Range(1, 10);

        Randomizer.Randomize(EasyFruits);
        for(int i = 0; i < EasyFruits.Length; i++)
        {
            if(i > targetNumber - 1)
                EasyFruits[i].SetActive(false);
        }

    }

    void EasyDrop(GameObject go)
    {
        ray = control.Cam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 100, DropLayer.value))
        {
            SetControl(false);
            ImportantAction();
            gameSummary = "0," + targetNumber + "," + go.name;
            if(DataManager.IsNAGame)
            {
                InGameStars.Show(LevelPos);
                NAEnd();
                go.transform.SetParent(GoodPosition);
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localScale = Vector3.one;
                return;
            }

            if(go.name.Equals(targetNumber.ToString()))
            {
                InGameStars.Show(LevelPos);
                ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
                go.transform.SetParent(GoodPosition);
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localScale = Vector3.one;
                return;
            }
            else
            {
                ConversationUI.ShowText(LevelKeyName + Easy + Wrong, () => SetControl(true));
            }
        }

        for(int i = 0; i < startPos.Length; i++)
            Numbers[i].transform.position = startPos[i];
    }
}
