using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicText : MonoBehaviour
{
    public Text Message;
    public Image Background;

    void OnEnable()
    {
        OptionsManager.TextChange += UpdateText;

        Font f;
        OptionsManager.TextColor c;
        OptionsManager.ManualUpdate(out f, out c);
        UpdateText(f, c);
    }

    void OnDisable()
    {
        OptionsManager.TextChange -= UpdateText;
    }

    void UpdateText(Font f, OptionsManager.TextColor c)
    {
        Message.font = f;
        Message.color = c.FontColor;
        Background.color = c.BackColor;
    }

}
