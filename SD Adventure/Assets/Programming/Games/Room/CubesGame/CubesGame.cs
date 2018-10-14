using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesGame : BaseGame
{
    [Header("Cubes Game")]
    public Collider[] Containers;
    public GameObject[] Cubes;
    public string[] Ids;


    public override void Start()
    {
        base.Start();
        SetContainers();
    }

    void SetContainers()
    {
        Randomizer.Randomize(Ids);
        Randomizer.Randomize(Cubes);

        for(int i = 0; i < Containers.Length; i++)
        {
            Containers[i].name = Ids[i];
        }

        int idsLenght = 0;
        for(int i = 0; i < Cubes.Length; i++)
        {
            Cubes[i].name = Ids[idsLenght];

            idsLenght++;
            if(idsLenght > Ids.Length - 1)
                idsLenght = 0;
        }
    }

    public override void Complete()
    {
        int hits = 0;
        CheckContainer(0, ref hits);
        CheckContainer(1, ref hits);
        CheckContainer(2, ref hits);

        if(hits == Cubes.Length)
            Debug.Log("Flawless Victory");
        else
            Debug.Log("Epic Fail");

        base.Complete();
    }

    void CheckContainer(int ind, ref int hits)
    {
        for(int i = 0; i < Cubes.Length; i++)
        {
            if(Containers[ind].name.Equals(Cubes[i].name) && Containers[ind].bounds.Contains(Cubes[i].transform.position))
                hits++;
        }
    }

}
