using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IObjectIDController {

    public string id;
    public string objectId{
        get{
            return id;
        }
        set{
            id = value;
        }
    }
}
