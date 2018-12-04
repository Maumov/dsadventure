using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject Cam;
    Transform playerCam;
    public Transform FinalPos;

    private void Start()
    {
        playerCam = FindObjectOfType<PlayerCamera>().transform;
    }

    [ContextMenu("asd")]
    public void AnimCamera()
    {
        Cam.transform.position = playerCam.position;
        Cam.SetActive(true);
        LeanTween.move(Cam, FinalPos.position, 3).setEase(LeanTweenType.easeInOutSine);
    }


}
