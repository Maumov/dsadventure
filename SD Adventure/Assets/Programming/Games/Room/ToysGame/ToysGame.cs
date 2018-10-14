using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToysGame : CubesGame
{
    int max;
    int[] amounts = new int[3];

    protected override void Initialize()
    {
        max = Random.Range((int)(Cubes.Length * 0.75f), Cubes.Length - 1);
        amounts[0] = Random.Range(1, max);
        amounts[1] = Random.Range(1, max - amounts[0]);
        amounts[2] = max - amounts[0] - amounts[1];
    }

    protected override void CompleteValidations()
    {
        Debug.Log(CheckContainers(0) && CheckContainers(1) && CheckContainers(2));
    }

    bool CheckContainers(int ind)
    {
        int a = 0;
        for(int i = 0; i < Cubes.Length; i++)
        {
            if(Containers[ind].bounds.Contains(Cubes[i].transform.position))
                a++;
        }
        return a == amounts[ind];
    }
}
