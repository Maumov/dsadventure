using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateFiles : GenericMenu
{
    [Header("CreateFiles")]
    public Transform AvatarPivot;
    public Text Counter;
    public GameObject CreateButton;

    GameObject[] avatarList;
    int avatarId;
    string fileName;
    string age;

    void Start()
    {
        avatarList = new GameObject[AvatarDatabase.ModelList.Length];
        for(int i = 0; i < avatarList.Length; i++)
        {
            avatarList[i] = Instantiate(AvatarDatabase.ModelList[i]);
            avatarList[i].transform.SetParent(AvatarPivot);
            avatarList[i].transform.localPosition = Vector3.zero;
            avatarList[i].transform.localEulerAngles = Vector3.zero;
            avatarList[i].transform.localScale = Vector3.one;
            avatarList[i].layer = 5;
            avatarList[i].SetActive(false);
        }
        avatarList[0].SetActive(true);
        Counter.text = "1/" + avatarList.Length;
    }


    public void SetName(string str)
    {
        fileName = str;
        CreateButton.SetActive(!string.IsNullOrEmpty(age) && !string.IsNullOrEmpty(fileName));
    }

    public void SetAge(string str)
    {
        age = str;
        CreateButton.SetActive(!string.IsNullOrEmpty(age) && !string.IsNullOrEmpty(fileName));
    }

    public void ChangeAvatar(int dir)
    {
        avatarId += dir;
        if(avatarId > avatarList.Length - 1)
            avatarId = 0;

        if(avatarId < 0)
            avatarId = avatarList.Length - 1;

        for(int i = 0; i < avatarList.Length; i++)
            avatarList[i].SetActive(false);
        avatarList[avatarId].SetActive(true);

        Counter.text = (avatarId + 1) + "/" + avatarList.Length;
    }

    public void Create()
    {
        if(string.IsNullOrEmpty(fileName))
            return;

        FileData newFile = new FileData(fileName, age, avatarId);
        DataManager.AddFile(newFile);
        DataManager.SetSelectedFile(DataManager.GetAllFiles().Length - 1);
        SceneLoader.LoadScene(DataManager.LastScene);
    }
}
