using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownProyector : MonoBehaviour
{
    Vector3 rot = new Vector3(90, 0, 0);

    private void LateUpdate()
    {
        transform.eulerAngles = rot;
    }
}
