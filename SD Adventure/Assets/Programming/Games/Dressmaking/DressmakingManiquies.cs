using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressmakingManiquies : BaseGame
{

    public Camera gameCam;
    [Header("Dressmaking")]
    public GameObject[] Maniquies;
    Vector3[] threeLayout;
    DragAndDrop control;

    [Header("Hard Settings")]
    public Transform[] FourLayout;
    public GameObject[] HardClothes;
    public Collider Container;

    [Header("Easy Settings")]
    public GameObject EasyClothes;
    public Collider[] ManiquiesContainers;
    public GameObject[] ManiquiesClothes;
    int matches;

    protected override void Initialize()
    {
        threeLayout = new Vector3[3];
        for(int i = 0; i < threeLayout.Length; i++)
            threeLayout[i] = Maniquies[i].transform.position;

        //Randomizer.Randomize(threeLayout);
        //Randomizer.Randomize(FourLayout);

        control = FindObjectOfType<DragAndDrop>();

        if(DataManager.IsHardGame)
            InitHard();
        else
            InitEasy();

        Summary ();
    }

    protected override void Summary(){
        if (DataManager.IsHardGame) {
            for (int i = 0; i < HardClothes.Length; i++) {
                Vector2 pos = ScreenCoordinates(gameCam,HardClothes [i].transform.position);
                gameObjets += "" + i + "," + pos.x + "," + pos.y+";";
            }

            Vector2 p = ScreenCoordinates(gameCam,Container.transform.position);
            gameSockets += "0"+"," + p.x + "," + p.y+";";


        } else {
            for (int i = 0; i < ManiquiesClothes.Length; i++) {
                Vector2 pos = ScreenCoordinates(gameCam,ManiquiesClothes [i].transform.position);
                gameObjets += "" + i + "," + pos.x + "," + pos.y+";";
            }
            for(int i = 0; i < ManiquiesContainers.Length; i++){
                Vector2 pos = ScreenCoordinates(gameCam,ManiquiesContainers [i].transform.position);
                gameSockets += "" + i +"," + pos.x+ "," + pos.y+";";
            }

        }
    }

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    void InitHard()
    {
        EasyClothes.SetActive(false);
        Maniquies[0].transform.parent.gameObject.SetActive(true);
        for(int i = 0; i < HardClothes.Length; i++)
            HardClothes[i].SetActive(true);

        int m = Random.Range(3, 5);
        if(m == 3)
        {
            Randomizer.Randomize(HardClothes);
            Maniquies[Maniquies.Length - 1].SetActive(false);
            HardClothes[HardClothes.Length - 1].SetActive(false);

            for(int i = 0; i < threeLayout.Length; i++)
                Maniquies[i].transform.position = threeLayout[i];
        }
        else
        {
            for(int i = 0; i < FourLayout.Length; i++)
                Maniquies[i].transform.position = FourLayout[i].position;
        }


    }

    void InitEasy()
    {
        EasyClothes.SetActive(true);
        for(int i = 0; i < HardClothes.Length; i++)
            HardClothes[i].SetActive(false);

        CompleteButton = null;

        Maniquies[Maniquies.Length - 1].SetActive(false);
        for(int i = 0; i < threeLayout.Length; i++)
            Maniquies[i].transform.position = threeLayout[i];

        control.OnDrop += CheckDrop;
    }

    void CheckDrop(GameObject go)
    {
        for(int i = 0; i < ManiquiesContainers.Length; i++)
        {
            if(ManiquiesContainers[i].bounds.Contains(go.transform.position))
            {
                if(DataManager.IsNAGame)
                {
                    NAEnd();
                    go.SetActive(false);
                    ManiquiesClothes[i].SetActive(true);
                    SetControl(false);
                    return;
                }
                if(ManiquiesContainers[i].name.Contains(go.name))
                {
                    Debug.Log("Match");
                    matches++;
                    gameSummary += ManiquiesContainers [i].name + "," + go.name;
                    go.SetActive(false);
                    ManiquiesClothes[i].SetActive(true);
                    if(matches == 3)
                    {
                        InGameStars.Show(LevelPos);
                        //gameSummary = "3 coincidencias";
                        ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
                    }
                }
                else
                {
                    SetControl(false);
                    ConversationUI.ShowText(LevelKeyName + Easy + Wrong, () => SetControl(true));
                }
                ImportantAction();
                return;
            }
        }
    }

    public void CheckHard()
    {
        ImportantAction();
    }

    public void Check()
    {
        CompleteButton.SetActive(false);
        if(DataManager.IsNAGame)
        {
            NAEnd();
            SetControl(false);
            return;
        }

        gameSummary += "0";
        for(int i = 0; i < HardClothes.Length; i++)
        {
            if(Container.bounds.Contains(HardClothes[i].transform.position)){
                gameSummary +=   "," +HardClothes[i];
                matches++;
            }
                
        }
        gameSummary += ";";

        if(matches == 2)
        {
            InGameStars.Show(LevelPos);
            ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
        }
        else
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
    }
}
