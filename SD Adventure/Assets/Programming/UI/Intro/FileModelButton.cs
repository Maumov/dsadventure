using System.Collections;
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

    IntroManager manager;
    FilesUI fileManager;

    private void Start()
    {
        manager = FindObjectOfType<IntroManager>();
        fileManager = FindObjectOfType<FilesUI>();
    }

    public void Set(FileData file, int id, Transform avatar, Sprite scene)
    {
        filedId = id;
        if(id > -1)
        {
            FileName.text = file.FileName;
            AvatarPivot.gameObject.SetActive(true);
            avatar.SetParent(AvatarPivot);
            avatar.transform.localPosition = Vector3.zero;
            avatar.transform.localEulerAngles = Vector3.zero;
            avatar.transform.localScale = Vector3.one;

            Background.sprite = scene;
            AvatarEmpty.enabled = false;
        }
        else
        {
            FileName.text = "Nuevo Personaje";
            AvatarPivot.gameObject.SetActive(false);
            Background.sprite = EmptyBackground;
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
