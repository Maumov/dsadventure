using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPopUp : MonoBehaviour
{
    public Image Black;
    public GameObject Content;

    public Text Message;
    const string DefaultText = "¿Estas seguro?";
    System.Action<bool> answer;

    static ConfirmationPopUp instance;

    private void Start()
    {
        instance = this;
        transform.SetAsLastSibling();
    }

    public static void GetConfirmation(string msg, System.Action<bool> callback)
    {
        instance.Confirmation(msg, callback);
    }

    void Confirmation(string msg, System.Action<bool> callback)
    {
        answer = callback;
        Content.SetActive(true);
        Content.transform.localScale = Vector3.zero;
        LeanTween.scale(Content, Vector3.one, 0.25f).setEase(LeanTweenType.easeOutBack);

        Black.enabled = true;

        if(string.IsNullOrEmpty(msg))
            msg = DefaultText;
        Message.text = msg;
    }

    public void PlayerAnswer(bool sw)
    {
        if(answer != null)
            answer(sw);
        LeanTween.scale(Content, Vector3.zero, 0.25f).setEase(LeanTweenType.easeInBack).onComplete += ()=>
        {
            Content.SetActive(false);
            Black.enabled = false;

        };
    }


}
