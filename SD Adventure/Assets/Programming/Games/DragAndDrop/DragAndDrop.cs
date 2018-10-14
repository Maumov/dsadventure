using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    protected Camera Cam;

    [HideInInspector]
    public bool Active = true;
    protected GameObject dragObject;
    Collider dragCollider;
    Rigidbody dragRigidbody;

    Vector3 mousePos;
    Vector3 targetPos;

    public LayerMask PickLayer;
    public LayerMask DragLayer;

    public virtual void Start()
    {
        Cam = GetComponent<Camera>();
    }

    void Update()
    {

        if(!Active)
            return;
        mousePos = Input.mousePosition;
        ray = Cam.ScreenPointToRay(mousePos);
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit, 50, PickLayer.value))
            {
                if(hit.collider.CompareTag("Drag"))
                {
                    dragObject = hit.collider.gameObject;
                    dragCollider = hit.collider;
                    dragRigidbody = hit.rigidbody;
                    dragCollider.enabled = false;
                    dragRigidbody.isKinematic = true;
                    BeginDrag();
                }
            }
        }
        if(Input.GetMouseButton(0) && dragObject != null)
        {
            if(Physics.Raycast(ray, out hit, 50, DragLayer.value))
            {
                dragObject.transform.position = hit.point;
            }
        }

        if(Input.GetMouseButtonUp(0) && dragObject != null)
        {
            Drop();
        }
    }

    public virtual void Drop()
    {
        dragCollider.enabled = true;
        dragRigidbody.isKinematic = false;
        dragObject = null;
        dragCollider = null;
        dragRigidbody = null;
    }

    public virtual void BeginDrag()
    {

    }
}
