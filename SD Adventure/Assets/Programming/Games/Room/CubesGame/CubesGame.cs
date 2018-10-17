using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesGame : BaseGame
{
    [Header("Cubes Game")]
    public Collider[] Containers;
    public MeshRenderer[] Cubes;
    public Material[] Ids;
    DragAndDrop control;

    protected override void Start()
    {
        base.Start();
        control = FindObjectOfType<DragAndDrop>();
        control.Active = false;
    }

    protected override void Initialize()
    {
        SetContainers();
    }

    public override void StartGame()
    {
        base.StartGame();
        control.Active = true;
    }

    void SetContainers()
    {
        //Randomizer.Randomize(Ids);
        Randomizer.Randomize(Cubes);

        for(int i = 0; i < Containers.Length; i++)
        {
            Containers[i].name = Ids[i].name;
        }

        int idsLenght = 0;
        for(int i = 0; i < Cubes.Length; i++)
        {
            Cubes[i].name = Ids[idsLenght].name;
            Cubes[i].material = Ids[idsLenght];

            idsLenght++;
            if(idsLenght > Ids.Length - 1)
                idsLenght = 0;
        }
    }

    protected override void CompleteValidations()
    {
        int hits = 0;
        for(int i = 0; i < Containers.Length; i++)
            CheckContainer(i, ref hits);


        if(hits == Cubes.Length)
            Debug.Log("Flawless Victory");
        else
            Debug.Log("Epic Fail");
    }

    void CheckContainer(int ind, ref int hits)
    {
        for(int i = 0; i < Cubes.Length; i++)
        {
            if(Containers[ind].name.Equals(Cubes[i].name) && Containers[ind].bounds.Contains(Cubes[i].transform.position))
                hits++;
        }
    }

    public void CubeDrop()
    {
        EnableCompleteButton();
    }

}
