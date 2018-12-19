﻿using System.Collections;
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

        if (DataManager.IsHardGame)
        { 
            InitHard ();
        }else {
            InitEasy();
        }  

        Summary ();
    }


    protected override void Summary() { 
        if (DataManager.IsHardGame) {
            for (int i = 0; i < HardClothes.Length; i++) {
                Vector2 pos = Camera.main.ViewportToScreenPoint (HardClothes [i].transform.position);
                gameObjets += "ObjectID:" + i + 1 + ":" + pos.x + " , " + pos.y;
            }

            Vector2 p = Camera.main.ViewportToScreenPoint (HardContainer.transform.position);
            gameSockets += "SocketID:" + p.x + " , " + p.y;
                
        } else {
            for(int i = 0; i < EasyClothes.Length; i++){
                Vector2 pos = Camera.main.ViewportToScreenPoint (EasyClothes [i].transform.position);
                gameObjets += "ObjectID:"+ EasyClothes [i].name +":" + pos.x+ " , " + pos.y;
            }

            for(int i = 0; i < EasyContainers.Length; i++){
                Vector2 pos = Camera.main.ViewportToScreenPoint (EasyContainers [i].transform.position);
                gameSockets += "SocketID" + i +":" + pos.x+ " , " + pos.y;
            }
        }

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
        if(DataManager.IsNAGame)
        {
            NAEnd();
            SetControl(false);
            return;
        }

        for(int i = 0; i < EasyContainers.Length; i++)
        {
            for(int j = 0; j < EasyClothes.Length; j++)
            {
                if(EasyContainers[i].bounds.Contains(EasyClothes[j].transform.position))
                    gameSummary += "Caja " + i + " tiene " + EasyClothes[j].name + ";";
            }
        }

        for(int i = 0; i < EasyContainers.Length; i++)
        {
            if(!EasyContainers[i].bounds.Contains(EasyClothes[i].transform.position))
            {
                ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
                return;
            }
        }
        InGameStars.Show(LevelPos);
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
        if(DataManager.IsNAGame)
        {
            NAEnd();
            SetControl(false);
            return;
        }
        if(HardContainer.bounds.Contains(HardClothes[0].transform.position))
        {
            InGameStars.Show(LevelPos);
            gameSummary = "Correcta";
            ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
        }
        else
        {
            gameSummary = "Incorrecta";
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
        }
    }
}
