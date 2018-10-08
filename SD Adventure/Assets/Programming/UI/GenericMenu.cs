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
    }

    public virtual void Hide()
    {
        if(Content != null)
            Content.SetActive(false);
        if(OpenButton != null)
            OpenButton.SetActive(true);
    }
}