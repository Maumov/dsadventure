using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateFiles : GenericMenu
{
    string fileName;
    int avatarId;

    public void SetName(string str)
    {
        fileName = str;
    }

    public void SetAvatarId(Toggle t)
    {
        if(t.isOn)
            avatarId = t.transform.GetSiblingIndex();
    }


    public void Create()
    {
        if(string.IsNullOrEmpty(fileName))
            return;

        FileData newFile = new FileData()
        {
            FileName = fileName,
            AvatarId = avatarId,
            LastScene = ""
        };
        DataManager.AddFile(newFile);
        DataManager.SetSelectedFile(DataManager.GetAllFiles().Length - 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
    }
}
