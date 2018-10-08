using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileModelButton : MonoBehaviour
{
    public Text FileInfo;
    int fildeId;

    public void Set(FileData file, int id)
    {
        FileInfo.text = "File name:\n " + file.FileName + "\nAvtar #" + file.AvatarId;
        fildeId = id;
    }

    public void Select()
    {
        DataManager.SetSelectedFile(fildeId);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
    }

}
