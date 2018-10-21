using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicText : MonoBehaviour
{
    public Image Background;
    public Text Message;

    private void Start()
    {
        Font f;
        OptionsManager.TextColor c;
        OptionsManager.ManualUpdate(out f, out c);
        UpdateText(f, c);

        OptionsManager.TextChange += UpdateText;
    }

    private void OnDestroy()
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
