using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObjectEvent : InteractionObject
{
    public UnityEvent ActionEvent;

    public override void Action()
    {
        base.Action();
        ActionEvent.Invoke();
    }
}

public class InteractionObject : MonoBehaviour
{
    public virtual void Action()
    {

    }
}