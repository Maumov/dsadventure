using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GenericMenu[] Menus;
    public GameObject BackButton;
    GenericMenu current;
    WaitForSeconds changeTime = new WaitForSeconds(0.75f);

    void Start()
    {
        SetCurrentMenu(0);
    }

    public void SetCurrentMenu(int id)
    {
        if(current != null)
            current.Hide();

        StartCoroutine(ChangeDelay(id));
    }

    public void Back()
    {
        if(current.BackMenu != null)
        {
            for(int i = 0; i < Menus.Length; i++)
                if(Menus[i].Equals(current.BackMenu))
                    SetCurrentMenu(i);
        }
        else if(!string.IsNullOrEmpty(current.BackScene))
        {
            SceneLoader.LoadScene(current.BackScene);
        }
    }

    IEnumerator ChangeDelay(int id)
    {
        yield return changeTime;
        current = Menus[id];
        current.Show();

        if(current.BackMenu == null && string.IsNullOrEmpty(current.BackScene))
            BackButton.SetActive(false);
        else
            BackButton.SetActive(true);

    }

}
