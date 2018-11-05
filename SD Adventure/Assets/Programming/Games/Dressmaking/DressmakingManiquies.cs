using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressmakingManiquies : BaseGame
{
    [Header("Dressmaking")]
    public GameObject[] Maniquies;
    Vector3[] threeLayout;
    DragAndDrop control;

    [Header("Hard Settings")]
    public Transform[] FourLayout;
    public GameObject[] HardClothes;

    [Header("Easy Settings")]
    public GameObject EasyClothes;
    public Collider[] ManiquiesContainers;
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
    }

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    void InitHard()
    {
        EasyClothes.SetActive(false);
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
                if(ManiquiesContainers[i].name.Contains(go.name))
                {
                    Debug.Log("Match");
                    matches++;
                    go.SetActive(false);
                    if(matches == 3)
                        ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
                }
                else
                {
                    SetControl(false);
                    ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ()=> SetControl(true));
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
        matches++;
        if(matches == 2)
            ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
        else
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
    }
}
