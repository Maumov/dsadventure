using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilesUI : GenericMenu
{
    public FileModelButton[] Buttons;
    int[] currentFiles;
    public Sprite SceneBackground;
    public Text Counter;

    FileData[] files;

    void Start()
    {
        UpdateFiles();
    }

    public void ChangeFiles(int dir)
    {
        SetIndex(1, dir);

        currentFiles[0] = currentFiles[1] - 1;
        currentFiles[2] = currentFiles[1] + 1;

        Counter.text = (currentFiles[1] + 2) + "/" + (files.Length + 1);
        SetOptions();
    }

    public void UpdateFiles()
    {
        files = DataManager.GetAllFiles();
        currentFiles = new int[] { files.Length - 1, -1, 0 };
        ChangeFiles(0);
    }

    void SetIndex(int ind, int dir)
    {
        currentFiles[ind] += dir;

        if(currentFiles[ind] > files.Length - 1)
            currentFiles[ind] = files.Length - 1;

        if(currentFiles[ind] < -1)
            currentFiles[ind] = -1;
    }

    void SetOptions()
    {
        if(files == null || files.Length == 0)
        {
            Buttons[0].Hide();
            Buttons[2].Hide();
            Buttons[1].Set(null, -1, SceneBackground);
            Buttons[1].SetInFront(true);
            return;
        }

        for(int i = 0; i < Buttons.Length; i++)
        {
            if(currentFiles[i] == -2 || currentFiles[i] == files.Length)
            {
                Buttons[i].Hide();
                continue;
            }

            if(currentFiles[i] != -1)
                Buttons[i].Set(files[currentFiles[i]], currentFiles[i], SceneBackground);
            else
                Buttons[i].Set(null, -1, SceneBackground);
            Buttons[i].SetInFront(i == 1);
        }
    }
}
