using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    bool active;
    float timeActive;
    public CanvasGroup CanvasContent;

    private void Update()
    {
        if(!active)
            return;

        if(Input.GetMouseButtonDown(0) && Time.time - timeActive > 1)
        {
            SetState(false);
        }

    }

    public void SetState(bool sw)
    {
        if(sw)
        {
            CanvasContent.gameObject.SetActive(true);
            timeActive = Time.time;
        }
        float from = sw ? 0 : 1;
        float to = sw ? 1 : 0;
        active = sw;
        LeanTween.value(gameObject, SetCanvas, from, to, 0.5f).onComplete += () =>
        {
            if(!sw)
                CanvasContent.gameObject.SetActive(false);
        };
    }

    void SetCanvas(float f)
    {
        CanvasContent.alpha = f;
    }

}
