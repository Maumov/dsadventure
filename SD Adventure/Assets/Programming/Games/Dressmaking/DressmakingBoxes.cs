﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressmakingBoxes : BaseGame
{
    DragAndDrop control;
    Vector3[] pos;

    [Header("Dressmaking")]
    public string[] OnCompleteKeys;

    [Header("Dressmaking Easy")]
    public GameObject EasyContent;
    public Collider[] EasyContainers;
    public GameObject[] EasyClothes;

    [Header("Dressmaking Hard")]
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
        for(int i = 0; i < EasyContainers.Length; i++)
        {
            if(!EasyContainers[i].bounds.Contains(EasyClothes[i].transform.position))
            {
                Debug.Log("Bad");
                return;
            }
        }
        Win();
    }

    void InitHard()
    {
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
            Win();
        else
            Debug.Log("Bad");
    }

    void Win()
    {
        for(int i = 0; i < OnCompleteKeys.Length; i++)
            DataManager.AddProgressKey(OnCompleteKeys[i], 1);

        SceneLoader.LoadScene(BaseScene);
    }
}