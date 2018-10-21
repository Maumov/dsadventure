﻿using System.Collections;
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
    public string Message;

    public virtual void Action() { }

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