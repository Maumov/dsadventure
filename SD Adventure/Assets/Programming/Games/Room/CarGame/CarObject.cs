using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObject : MonoBehaviour
{
    [HideInInspector]
    [System.NonSerialized]
    public Vector3 targetPos;
    bool follow;
    Rigidbody body;
    CarGame manager;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        manager = FindObjectOfType<CarGame>();
        GetComponentInChildren<UnityEngine.UI.Text>().text = (transform.GetSiblingIndex() + 1).ToString();
    }
    public void SetTarget(Vector3 target)
    {
        targetPos = target;
        targetPos.y = transform.position.y;
        if(Vector3.SqrMagnitude(transform.position - targetPos) > 0.1f)
            follow = true;
    }

    void FixedUpdate()
    {
        if(follow)
        {
            if(Vector3.SqrMagnitude(transform.position - targetPos) > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
                //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2);
                body.MovePosition(transform.position + transform.forward * Time.deltaTime * 1.5f);
            }
            else
            {
                follow = false;
                manager.CarStoped(transform);
            }
        }
    }

    public void Disable()
    {
        body.isKinematic = true;
        GetComponentInChildren<TextMesh>().gameObject.SetActive(false);
    }

}
