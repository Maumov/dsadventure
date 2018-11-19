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
    public Color BlueBackground;
    public Image Stars;
    public Sprite[] FillStars;

    IntroManager manager;
    FilesUI fileManager;
    GameObject[] avatarList;

    public Sprite[] ScenePictures;
    public LevelStars[] Levels;

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

    public void Set(FileData file, int id)
    {
        CreateAvatars();

        gameObject.SetActive(true);

        for(int i = 0; i < avatarList.Length; i++)
            avatarList[i].SetActive(false);

        filedId = id;
        Stars.sprite = FillStars[0];
        if(id > -1)
        {
            FileName.text = file.FileName;
            avatarList[file.AvatarId].SetActive(true);

            for(int i = 0; i < ScenePictures.Length; i++)
                if(file.LastScene.Equals(ScenePictures[i].name))
                    Background.sprite = ScenePictures[i];

            int starCount = 0;
            for(int i = 0; i < Levels.Length; i++)
            {
                if(file.LastScene.Equals(Levels[i].SceneName))
                {
                    for(int j = 0; j < Levels[i].GameNames.Length; j++)
                    {
                        if(file.CheckKey(Levels[i].GameNames[j]))
                            starCount++;
                    }
                    Stars.sprite = FillStars[starCount];
                    break;
                }
            }

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

    public void Hide()
    {
        gameObject.SetActive(false);
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
            SceneLoader.LoadScene(DataManager.LastScene);
        }
        else
        {
            manager.SetCurrentMenu(2);
        }
    }

    public void Delete()
    {
        ConfirmationPopUp.GetConfirmation("¿Estas seguro de borrar este archivo?", Confirm);
    }

    void Confirm(bool sw)
    {
        if(!sw)
            return;

        DataManager.DeleteFile(filedId);
        fileManager.UpdateFiles();
    }
}


[System.Serializable]
public class LevelStars
{
    public string SceneName;
    public string[] GameNames;
}
