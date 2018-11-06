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

public class InteractionObject : Interaction
{
    public SpriteRenderer Icon;
    public Sprite Hidden;
    public Sprite Shown;
    public string Message;

    public override void BeginInteraction()
    {
        base.BeginInteraction();
        ShowUI();
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
        HideUI();
    }

    public virtual void ShowUI()
    {
        if(Icon == null)
            return;
        Icon.sprite = Shown;
        if(!string.IsNullOrEmpty(Message))
            InfoText.Instance.Show(Message);
    }

    public virtual void HideUI()
    {
        if(Icon == null)
            return;
        Icon.sprite = Hidden;
        if(!string.IsNullOrEmpty(Message))
            InfoText.Instance.Hide();
    }
}

public class Interaction : MonoBehaviour
{
    public virtual void Action() { }

    public virtual void BeginInteraction() { }

    public virtual void EndInteraction() { }
}