using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 1;
    Transform cam;
    [HideInInspector]
    [System.NonSerialized]
    public bool ControlState = true;

    //Inputs
    Vector3 axis;
    Vector3 CamFaceY;
    Vector3 CamFaceX;

    //Components
    MobileControlRig inputs;
    CharacterController controller;
    Vector3 move;
    Interaction interaction;
    ButtonHandler actionButton;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        actionButton = FindObjectOfType<ButtonHandler>();
        inputs = FindObjectOfType<MobileControlRig>();
        cam = FindObjectOfType<PlayerCamera>().transform;
        actionButton.SetState(false);
    }

    private void Start()
    {
        Helper h = FindObjectOfType<Helper>();
        if(h != null && h.PlayerSpeed != -1)
            MovementSpeed = h.PlayerSpeed;
    }

    private void Update()
    {
        if(ControlState != inputs.gameObject.activeInHierarchy)
            inputs.gameObject.SetActive(ControlState);
        if(!ControlState)
            return;

        GetInputs();
        Movement();
    }

    void GetInputs()
    {
        axis.Set(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0, CrossPlatformInputManager.GetAxisRaw("Vertical"));
        axis.Normalize();

        if(CrossPlatformInputManager.GetButtonDown("Action") && interaction != null)
            interaction.Action();
    }

    public void ForceInteraction(Interaction i)
    {
        i.Action();
    }

    void Movement()
    {
        CamFaceY = cam.forward;
        CamFaceY.y = 0;
        CamFaceY.Normalize();

        CamFaceX = cam.right;
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
        interaction = other.GetComponent<Interaction>();
        interaction.BeginInteraction();
        actionButton.SetState(interaction != null);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.Equals(interaction.gameObject))
        {
            if(interaction != null)
                interaction.EndInteraction();
            interaction = null;
            actionButton.SetState(false);
        }
    }

}