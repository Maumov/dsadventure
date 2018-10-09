using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private void Awake ()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update ()
    {
        GetInputs();
        Movement();
    }

    void GetInputs ()
    {
        axis.Set(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void Movement ()
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

}