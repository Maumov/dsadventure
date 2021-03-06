﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToysGame : CubesGame
{
    int max;
    int[] amounts = new int[3];

    public Text[] Guides;

    protected override void Initialize()
    {
        max = Random.Range((int)(Cubes.Length * 0.8f), Cubes.Length - 1);
        amounts[0] = Random.Range(1, (int)(max * 0.8f));
        amounts[1] = Random.Range(1, max - amounts[0] - 1);
        amounts[2] = max - amounts[0] - amounts[1];

        for(int i = 0; i < Containers.Length; i++)
        {
            Guides[i].text = amounts[i].ToString();
        }

        Summary ();
    }

//    protected override void Summary(){
//        for (int i = 0; i < Cubes.Length; i++) {
//            Vector2 pos = Camera.main.ViewportToScreenPoint (Cubes[i].transform.position);
//            gameObjets += "ObjectID:" + i + ":" + pos.x + " , " + pos.y;
//        }
//
//        for(int i = 0; i < Containers.Length; i++){
//            Vector2 pos = Camera.main.ViewportToScreenPoint (Containers [i].transform.position);
//            gameSockets += "SocketID:" + i + pos.x + " , " + pos.y;
//        }
//    }

    protected override void CompleteValidations()
    {
        TimerState(false);
        int hits = 0;
        for(int i = 0; i < 3; i++)
        {
            if(CheckContainers(i))
                hits++;
        }

        if(hits > 1)
        {
            acomplishmentLevel = 2;
            DataManager.AddProgressKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 2);
        }
        else if(hits > 0)
        {
            acomplishmentLevel = 1;
            DataManager.AddProgressKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 1);
        }
        else
        {
            acomplishmentLevel = 0;
            DataManager.AddProgressKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 0);
        }
    }

    bool CheckContainers(int ind)
    {
        int a = 0;
        for(int i = 0; i < Cubes.Length; i++)
        {
            if(Containers[ind].bounds.Contains(Cubes[i].transform.position))
                a++;
        }
        string s = "" + ind;
        for(int i = 0; i < Cubes.Length; i++){
            if(Containers[ind].bounds.Contains(Cubes[i].transform.position)){
                s += "," + Cubes[i].name;
            }
        }
        s += ";";
        gameSummary += s;
        return a == amounts[ind];
    }
}
