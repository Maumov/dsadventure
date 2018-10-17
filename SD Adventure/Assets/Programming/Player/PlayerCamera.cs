using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform Target;
    public float FollowSpeed = 5;
    Vector3 offset;

    public Bounds cameraBounds;

    private void Start()
    {
        offset = transform.position - Target.position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position + offset, Time.deltaTime * FollowSpeed);
        Debug.Log(cameraBounds.Contains(transform.position));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(cameraBounds.center, cameraBounds.extents);
    }

}
