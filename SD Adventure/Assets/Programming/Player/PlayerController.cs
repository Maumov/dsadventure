﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 1;
    public Transform Cam;

    //Inputs
    Vector3 axis;
    Vector3 CamFaceY;
    Vector3 CamFaceX;

    //Components
    CharacterController controller;
    Vector3 move;
    InteractionObject interaction;
    ButtonHandler actionButton;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        actionButton = FindObjectOfType<ButtonHandler>();
        actionButton.SetState(false);
    }

    private void Update()
    {
        GetInputs();
        Movement();
    }

    void GetInputs()
    {
        axis.Set(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0, CrossPlatformInputManager.GetAxisRaw("Vertical"));
        axis.Normalize();

        if(CrossPlatformInputManager.GetButtonDown("Action") && interaction != null)
        {
            interaction.Action();
        }
    }

    void Movement()
    {
        CamFaceY = Cam.forward;
        CamFaceY.y = 0;
        CamFaceY.Normalize();

        CamFaceX = Cam.right;
        CamFaceX.y = 0;
        CamFaceX.Normalize();

        move = ((CamFaceY * axis.z) + (CamFaceX * axis.x)) * MovementSpeed;

        //move = axis.normalized * MovementSpeed;
        controller.Move((move + Vector3.down * 10) * Time.deltaTime);

        if(axis.sqrMagnitude > 0)
        {
            move.y = 0;
            transform.rotation = Quaternion.LookRotation(move.normalized);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        interaction = other.GetComponent<InteractionObject>();
        actionButton.SetState(interaction != null);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.Equals(interaction.gameObject))
        {
            interaction = null;
            actionButton.SetState(false);
        }
    }

}