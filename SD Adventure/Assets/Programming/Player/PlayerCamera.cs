using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform Target;
    public float FollowSpeed = 5;
    Vector3 offset;
    Vector3 nextPos;

    public Vector3 Min;
    public Vector3 Max;

    private void Start()
    {
        offset = transform.position - Target.position;
        nextPos = transform.position;
        Min.y = Max.y = transform.position.y;
    }

    private void LateUpdate()
    {
        nextPos = Vector3.Lerp(nextPos, Target.position + offset, Time.deltaTime * FollowSpeed);

        nextPos.x = Mathf.Clamp(nextPos.x, Min.x, Max.x);
        nextPos.y = Mathf.Clamp(nextPos.y, Min.y, Max.y);
        nextPos.z = Mathf.Clamp(nextPos.z, Min.z, Max.z);

        transform.position = nextPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(Min + (Max - Min) * 0.5f, Max - Min);
    }

}
