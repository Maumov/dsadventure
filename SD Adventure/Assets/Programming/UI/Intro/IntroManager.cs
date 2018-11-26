using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public GenericMenu[] Menus;
    public GameObject BackButton;
    GenericMenu current;
    WaitForSeconds changeTime = new WaitForSeconds(0.75f);
    public GameObject Rights;

    public GraphicRaycaster Interaction;

    void Start()
    {
        SetCurrentMenu(0);
        StartCoroutine(OptionsDelay());
        StartCoroutine(FilesCheck());
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
        Interaction.enabled = false;
        if(id != 0)
            Rights.SetActive(false);

        yield return changeTime;
        current = Menus[id];
        current.Show();

        if(current.BackMenu == null && string.IsNullOrEmpty(current.BackScene))
            BackButton.SetActive(false);
        else
            BackButton.SetActive(true);

        if(id == 0)
            Rights.SetActive(true);

        yield return changeTime;
        Interaction.enabled = true;

    }

    IEnumerator OptionsDelay()
    {
        OptionsManager.ButtonState(false);
        WaitForSeconds t = new WaitForSeconds(1.25f);
        yield return t;
        OptionsManager.ButtonState(true);
    }


    public FileData[] files;

    IEnumerator FilesCheck()
    {
        bool wait = true;
        files = DataManager.GetAllFiles();
        for(int i = 0; i < files.Length; i++)
        {
            for(int j = 0; j < files[i].PendingJsonFiles.Count; j++)
            {
                wait = true;
                yield return StartCoroutine(StatsHandler.Server(files[i].PendingJsonFiles[j], (sw) =>
                {
                    if(sw)
                    {
                        files[i].PendingJsonFiles.RemoveAt(j);
                        j--;
                    }
                    wait = false;
                }));
                while(wait)
                    yield return null;
            }

            if(!string.IsNullOrEmpty(files[i].RatingPending))
            {
                wait = true;
                yield return StartCoroutine(StatsHandler.SendRating(files[i].RatingPending, (sw) =>
                {
                    if(sw)
                        files[i].RatingPending = string.Empty;

                    wait = false;
                }));
                while(wait)
                    yield return null;
            }

        }
        DataManager.Save();
        yield return null;
    }
}
