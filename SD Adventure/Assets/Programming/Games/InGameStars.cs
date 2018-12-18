using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameStars : MonoBehaviour
{
    public Transform InPos;
    public Transform OutPos;

    public Image S;
    public Sprite[] Stars;

    public Animator[] estrellas;

    static InGameStars instance;
    public static bool cancelStar;

    [ContextMenu("asdas")]
    public void asddsa (){
        Show (1);
    }

    public static void Show(int i)
    {
        if(instance == null)
        {
            instance = Instantiate(Resources.Load<InGameStars>("InGameStars"));
            //DontDestroyOnLoad(instance.gameObject);
        }

        if(cancelStar)
        {
            cancelStar = false;
            return;
        }
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.ShowStars(i));
    }

    IEnumerator ShowStars(int i)
    {
        FindObjectOfType<BaseGame>().TimerState(false);
        S.enabled = false;
        S.sprite = Stars[i - 1];
        S.transform.position = OutPos.position;

        for(int j = 0; j < i-1; j++){
            estrellas [j].SetTrigger ("On");
        }

        //LeanTween.move(S.gameObject, InPos.position, 0.5f);
        //yield return new WaitForSeconds(0.75f);
        S.transform.position = InPos.position;
        S.enabled = true;
        //LeanTween.scale(S.gameObject, Vector3.one * 1.5f, 0.25f).setEase(LeanTweenType.easeOutCubic);
        //yield return new WaitForSeconds(0.25f);
        S.sprite = Stars[i];
        yield return new WaitForSeconds (1f);

        estrellas [i-1].SetTrigger ("Pop");
        SfxManager.Play(SFXType.Star);
        LeanTween.scale(S.gameObject, Vector3.one, 0.25f).setEase(LeanTweenType.easeInOutSine);
        //yield return new WaitForSeconds(10f);
        //S.enabled = false;
        //LeanTween.move(S.gameObject, OutPos.position, 0.5f);
        yield return null;
    }

}
