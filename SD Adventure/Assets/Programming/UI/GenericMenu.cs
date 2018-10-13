using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMenu : MonoBehaviour
{

    public GameObject Content;
    public GameObject OpenButton;

    [Header("Back Info")]
    public GenericMenu BackMenu;
    public string BackScene;

    public virtual void Show()
    {
        if(Content != null)
            Content.SetActive(true);
        if(OpenButton != null)
            OpenButton.SetActive(false);

        OverrideTween();
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);
    }

    public virtual void Hide()
    {
        OverrideTween();
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).onComplete += HideDelay;
    }

    void HideDelay()
    {
        if(Content != null)
            Content.SetActive(false);
        if(OpenButton != null)
            OpenButton.SetActive(true);
    }

    void OverrideTween()
    {
        if(LeanTween.isTweening(gameObject))
            LeanTween.cancel(gameObject);
    }
}