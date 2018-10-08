using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GenericMenu[] Menus;
    public GameObject BackButton;
    GenericMenu current;

    void Start()
    {
        SetCurrentMenu(0);
    }

    public void SetCurrentMenu(int id)
    {
        if(current != null)
            current.Hide();

        current = Menus[id];
        current.Show();

        if(current.BackMenu == null && string.IsNullOrEmpty(current.BackScene))
            BackButton.SetActive(false);
        else
            BackButton.SetActive(true);
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
            UnityEngine.SceneManagement.SceneManager.LoadScene(current.BackScene);
        }

    }

}
