using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilesUI : GenericMenu
{
    public FileModelButton[] Buttons;
    int[] currentFiles;
    public Sprite SceneBackground;
    GameObject[] avatarList;
    FileData[] files;
    Transform tempParent;

    void Start()
    {
        tempParent = new GameObject("Avatar Parent").transform;
        tempParent.SetParent(transform.transform);
        tempParent.gameObject.SetActive(false);

        avatarList = new GameObject[AvatarDatabase.ModelList.Length];
        for(int i = 0; i < avatarList.Length; i++)
        {
            avatarList[i] = Instantiate(AvatarDatabase.ModelList[i]);
            avatarList[i].transform.SetParent(tempParent);
            avatarList[i].layer = 5;
            avatarList[i].SetActive(true);
        }

        UpdateFiles();
    }

    public void ChangeFiles(int dir)
    {
        SetIndex(0, dir);
        SetIndex(1, dir);
        SetIndex(2, dir);

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
            currentFiles[ind] = -1;
        if(currentFiles[ind] < -1)
            currentFiles[ind] = files.Length - 1;
    }

    void SetOptions()
    {
        for(int i = 0; i < Buttons.Length; i++)
        {
            if(currentFiles[i] != -1)
                Buttons[i].Set(files[currentFiles[i]], currentFiles[i], avatarList[files[currentFiles[i]].AvatarId].transform, SceneBackground);
            else
                Buttons[i].Set(null, -1, null, SceneBackground);
            Buttons[i].SetInFront(i == 1);
        }
    }
}
