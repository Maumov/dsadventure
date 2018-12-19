using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesGame : BaseGame
{
    [Header("Cubes Game")]
	public Camera GameCam;
    public Collider[] Containers;
    public CubeObject[] Cubes;
    DragAndDrop control;

    public int[] CheckValue;
    public bool RandomizeContainers;
    protected override void Start()
    {
        base.Start();
        control = FindObjectOfType<DragAndDrop>();
        control.Active = false;
    }

    protected override void Initialize()
    {
        SetContainers();

        Summary ();
    }

    protected override void Summary(){
        for (int i = 0; i < Cubes.Length; i++) {
			Vector2 pos = ScreenCoordinates(GameCam, Cubes[i].transform.position);
			gameObjets += "" + i + "," + pos.x + "," + pos.y +"," +Cubes[i].name+";";
        }

        for(int i = 0; i < Containers.Length; i++){
			Vector2 pos = ScreenCoordinates(GameCam, Containers [i].transform.position);
			gameSockets += "" + i+"," + pos.x + "," + pos.y +"," + Containers[i].name + ";";
        }
    }
    public override void StartGame()
    {
        base.StartGame();
        control.Active = true;
    }

    void SetContainers()
    {
        Randomizer.Randomize(Cubes);

        if(RandomizeContainers)
        {
            Vector3[] cp = new Vector3[Containers.Length];
            for(int i = 0; i < cp.Length; i++)
            {
                cp[i] = Containers[i].transform.position;
            }
            Randomizer.Randomize(cp);

            for(int i = 0; i < cp.Length; i++)
            {
                Containers[i].transform.position = cp[i];
            }
        }

        for(int i = 0; i < Containers.Length; i++)
        {
            Containers[i].name = i.ToString();
        }

        int idsLenght = 0;
        for(int i = 0; i < Cubes.Length; i++)
        {
            Cubes[i].Set(idsLenght);
            idsLenght = (int)Mathf.Repeat(idsLenght + 1, Containers.Length);
        }
    }

    protected override void CompleteValidations()
    {
        TimerState(false);
        int hits = 0;
        for(int i = 0; i < Containers.Length; i++)
            CheckContainer(i, ref hits);


        if(hits > CheckValue[0])
        {
            acomplishmentLevel = 2;
            DataManager.AddProgressKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 2);
        }
        else if(hits > CheckValue[1])
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

    void CheckContainer(int ind, ref int hits)
    {
        int each = 0;
        for(int i = 0; i < Cubes.Length; i++)
        {
            if(Containers[ind].name.Equals(Cubes[i].name) && Containers[ind].bounds.Contains(Cubes[i].transform.position))
            {
                hits++;
                each++;
            }
        }
		string s = "" + ind;
		for(int i = 0; i < Cubes.Length; i++){
			if(Containers[ind].bounds.Contains(Cubes[i].transform.position)){
				s += "," + Cubes[i].name;
			}
		}
		s += ";";
        gameSummary += s;
    }

    public void CubeDrop()
    {
        ImportantAction();
    }

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

}
