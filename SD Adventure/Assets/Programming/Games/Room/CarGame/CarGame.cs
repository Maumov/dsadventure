using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGame : BaseGame
{
    [Header("Car Game")]
    public Camera GameCam;
    public Collider[] Places;
    public CarObject[] Cars;
    public Transform[] StartPlaces;
    Ray ray;
    RaycastHit hit;
    CarObject current;

    int upwards;
    int downwards;

    public override void Start()
    {
        base.Start();
        Randomizer.Randomize(StartPlaces);
        for(int i = 0; i < Cars.Length; i++)
        {
            //Cars[i].transform.position = StartPlaces[i].position;
        }
    }

    void Update()
    {
        if(!enableControls)
            return;

        ray = GameCam.ScreenPointToRay(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit, 50))
            {
                if(hit.transform.CompareTag("Drag"))
                {
                    current = hit.transform.GetComponent<CarObject>();
                }
            }
        }

        if(current != null)
        {
            Physics.Raycast(ray, out hit, 50);
            current.SetTarget(hit.point);
        }

        if(Input.GetMouseButtonUp(0))
        {
            current = null;
        }
    }

    public void CarStoped(Transform car)
    {
        for(int i = 0; i < Places.Length; i++)
        {
            if(Places[i].bounds.Contains(car.position))
            {
                EnableCompleteButton();
                return;
            }
        }
    }

    public override void Complete()
    {
        base.Complete();
        for(int i = 0; i < Places.Length; i++)
        {
            if(Places[i].bounds.Contains(Cars[i].targetPos))
                upwards++;
        }
        System.Array.Reverse(Cars);

        for(int i = 0; i < Places.Length; i++)
        {
            if(Places[i].bounds.Contains(Cars[i].targetPos))
                downwards++;
        }

        System.Array.Reverse(Cars);

        if(upwards == 5 || downwards == 5)
        {
            Debug.Log("Perfect : Flawless Victory");
        }
        else
        {
            Debug.Log("You Lose");
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Room");
    }


}


public class Randomizer
{
    public static void Randomize<T>(T[] items)
    {
        System.Random rand = new System.Random();

        // For each spot in the array, pick
        // a random item to swap into that spot.
        for(int i = 0; i < items.Length - 1; i++)
        {
            int j = rand.Next(i, items.Length);
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
}