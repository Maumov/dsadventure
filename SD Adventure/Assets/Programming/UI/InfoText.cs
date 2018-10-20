using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    public GameObject Content;
    public Transform OutPos;
    public Transform InPos;
    public Text ItemText;

    public static InfoText Instance;


    private void Start()
    {
        Instance = this;
        Content.SetActive(false);
        Content.transform.position = OutPos.position;
    }

    public void Show(string msg)
    {
        ItemText.text = msg;
        Content.SetActive(true);
        LeanTween.move(Content, InPos.position, 0.25f);
    }

    public void Hide()
    {
        LeanTween.move(Content, OutPos.position, 0.25f).onComplete += () =>
              {
                  Content.SetActive(false);
              };
    }
}
