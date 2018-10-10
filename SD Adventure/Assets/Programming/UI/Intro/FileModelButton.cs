﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileModelButton : MonoBehaviour
{
    public Text FileName;
    public Image ExtraButton;
    public Sprite AddSprite;
    public Sprite SelectSprite;
    public Sprite EmptyBackground;
    public Image Background;
    public Image AvatarEmpty;
    public Transform AvatarPivot;
    public Image Black;
    public Image DeleteButton;
    int filedId;
    public Button button;
    public Color BlueBackground;

    IntroManager manager;
    FilesUI fileManager;
    GameObject[] avatarList;

    private void Start()
    {
        manager = FindObjectOfType<IntroManager>();
        fileManager = FindObjectOfType<FilesUI>();
        CreateAvatars();
    }

    void CreateAvatars()
    {
        if(avatarList != null)
            return;

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
    }

    public void Set(FileData file, int id, Sprite scene)
    {
        CreateAvatars();

        for(int i = 0; i < avatarList.Length; i++)
            avatarList[i].SetActive(false);

        filedId = id;
        if(id > -1)
        {
            FileName.text = file.FileName;
            avatarList[file.AvatarId].SetActive(true);
            Background.sprite = scene;
            Background.color = Color.white;
            AvatarEmpty.enabled = false;
        }
        else
        {
            FileName.text = "Nuevo Personaje";
            Background.sprite = EmptyBackground;
            Background.color = BlueBackground;
            AvatarEmpty.enabled = true;
        }
    }

    public void SetInFront(bool sw)
    {
        Black.enabled = !sw;
        button.interactable = sw;
        ExtraButton.enabled = sw;

        ExtraButton.sprite = filedId > -1 ? SelectSprite : AddSprite;

        if(filedId > -1 && sw)
            DeleteButton.enabled = true;
        else
            DeleteButton.enabled = false;

    }

    public void Select()
    {
        if(filedId > -1)
        {
            DataManager.SetSelectedFile(filedId);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Room");
        }
        else
        {
            manager.SetCurrentMenu(2);
        }
    }

    public void Delete()
    {
        DataManager.DeleteFile(filedId);
        fileManager.UpdateFiles();
    }

}
