using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    [HideInInspector]
    [System.NonSerialized]
    public Camera Cam;

    [HideInInspector]
    public bool Active = true;
    protected GameObject dragObject;
    Collider dragCollider;
    Rigidbody dragRigidbody;

    Vector3 mousePos;
    Vector3 targetPos;

    public LayerMask PickLayer;
    public LayerMask DragLayer;

    public event System.Action<GameObject> OnDrop;
    public event System.Action<GameObject> OnDrag;

    Vector2 dragStartPos;
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
            if(Physics.Raycast(ray, out hit, 100, PickLayer.value))
            {
                if(hit.collider.CompareTag("Drag"))
                {
                    dragObject = hit.collider.gameObject;
                    dragCollider = hit.collider;
                    dragRigidbody = hit.rigidbody;
                    dragCollider.enabled = false;
                    if(dragRigidbody != null)
                        dragRigidbody.isKinematic = true;
                    BeginDrag();
                    SfxManager.Play(SFXType.Pick);
                }
            }
        }
        if(Input.GetMouseButton(0) && dragObject != null)
        {
            if(Physics.Raycast(ray, out hit, 100, DragLayer.value))
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
        if(OnDrop != null)
            OnDrop(dragObject);

        StatsHandler.Instance.AddDrag(dragObject.name, dragStartPos, mousePos);

        dragCollider.enabled = true;
        if(dragRigidbody != null)
            dragRigidbody.isKinematic = false;
        dragObject = null;
        dragCollider = null;
        dragRigidbody = null;
    }

    public virtual void BeginDrag()
    {
        dragStartPos = mousePos;
        if(OnDrag != null)
            OnDrag(dragObject);
    }
}
