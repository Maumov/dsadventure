﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressmakingHanger : BaseGame
{
    Vector3[] startPos;
    public Camera gameCam;
    [Header("Dressmaking Hard")]
    public GameObject HardContent;
    public GameObject[] ClothesHard;
    public Collider Box;

    [Header("Dressmaking Easy")]
    public GameObject EasyContent;
    public GameObject[] ClothesEasy;
    public Collider Closet;
    public GameObject HangClothes;

    DragAndDrop control;

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();

        if(DataManager.IsHardGame)
            InitHard();
        else
            InitEasy();
    }

    protected override void Summary(){
        if (DataManager.IsHardGame) {
            for (int i = 0; i < ClothesHard.Length; i++) {
                Vector2 pos = ScreenCoordinates(gameCam,ClothesHard [i].transform.position);
                gameObjets += "" + i + "," + pos.x + "," + pos.y+";";
            }

            Vector2 p = ScreenCoordinates(gameCam,Box.transform.position);
            gameSockets += "0"+"," + p.x + "," + p.y +";";
        
        }else{
            for(int i = 0; i < ClothesEasy.Length; i++){
                Vector2 pos = ScreenCoordinates(gameCam,ClothesEasy [i].transform.position);
                gameObjets += ""+ ClothesEasy [i].name +"," + pos.x+ "," + pos.y+";";
            }
            Vector2 p = ScreenCoordinates(gameCam,Closet.transform.position);
            gameSockets += "0"+"," + p.x + "," + p.y +";";

        }
            
    }


    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    void InitHard()
    {
        EasyContent.SetActive(false);
        HardContent.SetActive(true);

        startPos = new Vector3[ClothesHard.Length];
        for(int i = 0; i < startPos.Length; i++)
        {
            startPos[i] = ClothesHard[i].transform.position;
            ClothesHard[i].transform.localScale = Vector3.one;
        }

        Randomizer.Randomize(startPos);

        for(int i = 0; i < startPos.Length; i++)
            ClothesHard[i].transform.position = startPos[i];

    }

    void InitEasy()
    {
        HardContent.SetActive(false);
        EasyContent.SetActive(true);

        control.OnDrop += OnDrop;

        startPos = new Vector3[ClothesEasy.Length];
        for(int i = 0; i < startPos.Length; i++)
            startPos[i] = ClothesEasy[i].transform.position;

        Randomizer.Randomize(startPos);

        for(int i = 0; i < startPos.Length; i++)
            ClothesEasy[i].transform.position = startPos[i];
    }

    void OnDrop(GameObject go)
    {
        for(int i = 0; i < startPos.Length; i++)
        {
            if(Closet.bounds.Contains(ClothesEasy[i].transform.position))
            {
                gameSummary = "Selecciono " + go.name;
                SetControl(false);
                if(DataManager.IsNAGame)
                {
                    NAEnd();
                    go.SetActive(false);
                    HangClothes.SetActive(true);
                    return;
                }
                if(ClothesEasy[i].name.Equals("Ok"))
                {
                    InGameStars.Show(LevelPos);
                    HangClothes.SetActive(true);
                    ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
                    ClothesEasy[i].SetActive(false);
                }
                else
                {
                    ConversationUI.ShowText(LevelKeyName + Easy + Wrong, () => SetControl(true));
                }
                ImportantAction();
            }
            ClothesEasy[i].transform.position = startPos[i];
        }
    }


    public void CheckHard()
    {
        SetControl(false);
        if(DataManager.IsNAGame)
        {
            NAEnd();
            return;
        }
        if(Box.bounds.Contains(ClothesHard[0].transform.position))
        {
            //gameSummary = "Correcta";
            InGameStars.Show(LevelPos);
            ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
        }
        else
        {
            //gameSummary = "Incorrecta";
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
        }
        for(int i = 0; i< ClothesHard.Length; i++){
            if(Box.bounds.Contains(ClothesHard[i].transform.position)){
                gameSummary += "0" +"," + i +";";
            }
        }

    }
}
