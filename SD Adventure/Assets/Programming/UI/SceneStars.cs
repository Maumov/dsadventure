using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneStars : MonoBehaviour
{
    public string[] Keys;
    public Sprite[] Stars;

    private void Start()
    {
        Image star = GetComponent<Image>();
        for(int i = 0; i < Keys.Length; i++)
        {
            if(DataManager.CheckProgressKey(Keys[i]))
                star.sprite = Stars[i];
        }
    }

}
