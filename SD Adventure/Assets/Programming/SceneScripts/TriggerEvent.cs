using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public string TargetTag;

    public UnityEvent Trigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(TargetTag))
        {
            if(Trigger != null)
                Trigger.Invoke();
            if(TargetTag.Equals("Drag"))
                SfxManager.Play(SFXType.Basket);
        }
    }
}
