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
        if(ActionEvent != null)
            ActionEvent.Invoke();
    }
}

public class InteractionObject : MonoBehaviour
{
    public SpriteRenderer Icon;
    public Sprite Hidden;
    public Sprite Shown;

    public virtual void Action() { }

    public virtual void ShowUI()
    {
        if(Icon == null)
            return;

    }
    public virtual void HideUI()
    {
        if(Icon == null)
            return;
    }
}