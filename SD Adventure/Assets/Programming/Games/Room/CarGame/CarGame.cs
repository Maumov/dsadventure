﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGame : BaseGame
{
    [Header("Car Game")]
    public Camera GameCam;
    public Collider[] Places;
    public CarObject[] Cars;
    public Transform[] StartPlaces;
    public LayerMask DragLayer;
    Ray ray;
    RaycastHit hit;
    CarObject current;

    int upwards;
    int downwards;

    public Transform Mark;

    protected override void Initialize()
    {
        Randomizer.Randomize(StartPlaces);
        for(int i = 0; i < Cars.Length; i++)
        {
            Cars[i].transform.position = StartPlaces[i].position;
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
                    Mark.gameObject.SetActive(true);
                }
            }
        }

        if(current != null)
        {
            if(Physics.Raycast(ray, out hit, 50, DragLayer.value))
            {
                current.SetTarget(hit.point);
                Mark.position = hit.point;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            current = null;
            Mark.gameObject.SetActive(false);
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

    protected override void CompleteValidations()
    {
        for(int i = 0; i < Places.Length; i++)
        {
            if(Places[i].bounds.Contains(Cars[i].transform.position))
                upwards++;
        }
        System.Array.Reverse(Cars);

        for(int i = 0; i < Places.Length; i++)
        {
            if(Places[i].bounds.Contains(Cars[i].transform.position))
                downwards++;
        }

        System.Array.Reverse(Cars);

        if(upwards == 5 || downwards == 5)
            Debug.Log("Perfect : Flawless Victory");
        else
            Debug.Log("You Lose");
    }
}