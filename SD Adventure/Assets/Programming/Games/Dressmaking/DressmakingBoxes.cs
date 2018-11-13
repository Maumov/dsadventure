using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressmakingBoxes : BaseGame
{
    DragAndDrop control;
    Vector3[] pos;

    [Header("Dressmaking Easy")]
    public GameObject EasyContent;
    public Collider[] EasyContainers;
    public GameObject[] EasyClothes;

    [Header("Dressmaking Hard")]
    public GameObject HardContent;
    public Collider HardContainer;
    public GameObject[] HardClothes;

    protected override void Initialize()
    {
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

    void InitEasy()
    {
        EasyContent.SetActive(true);
        HardContent.SetActive(false);
        //CompleteButton = null;
        pos = new Vector3[EasyContainers.Length];
        for(int i = 0; i < pos.Length; i++)
            pos[i] = EasyContainers[i].transform.position;

        //Randomizer.Randomize(pos);
        for(int i = 0; i < pos.Length; i++)
            EasyContainers[i].transform.position = pos[i];

        pos = new Vector3[EasyClothes.Length];
        for(int i = 0; i < pos.Length; i++)
            pos[i] = EasyClothes[i].transform.position;

        Randomizer.Randomize(pos);
        for(int i = 0; i < pos.Length; i++)
            EasyClothes[i].transform.position = pos[i];
    }

    public void CheckEasy()
    {
        ImportantAction();
    }

    public void CompleteEasy()
    {
        CompleteButton.SetActive(false);
        for(int i = 0; i < EasyContainers.Length; i++)
        {
            if(!EasyContainers[i].bounds.Contains(EasyClothes[i].transform.position))
            {
                ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
                return;
            }
        }
        ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
    }

    void InitHard()
    {
        EasyContent.SetActive(false);
        HardContent.SetActive(true);
        CompleteButton = null;

        pos = new Vector3[HardClothes.Length];
        for(int i = 0; i < pos.Length; i++)
            pos[i] = HardClothes[i].transform.position;

        Randomizer.Randomize(pos);
        for(int i = 0; i < pos.Length; i++)
            HardClothes[i].transform.position = pos[i];

    }

    public void CheckHard()
    {
        ImportantAction();
        if(HardContainer.bounds.Contains(HardClothes[0].transform.position))
            ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
        else
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
    }
}
