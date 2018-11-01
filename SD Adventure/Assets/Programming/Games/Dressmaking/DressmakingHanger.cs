using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressmakingHanger : BaseGame
{
    [Header("Dressmaking")]
    public string[] OnCompleteKeys;
    Vector3[] startPos;

    [Header("Dressmaking Hard")]
    public GameObject HardContent;
    public GameObject[] ClothesHard;
    public Collider Box;

    [Header("Dressmaking Easy")]
    public GameObject EasyContent;
    public GameObject[] ClothesEasy;
    public Collider Closet;

    DragAndDrop control;

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();

        if(DataManager.IsHardGame)
            InitHard();
        else
            InitEasy();
    }

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    void InitHard()
    {
        EasyContent.SetActive(false);
        HardContent.SetActive(true);

        startPos = new Vector3[ClothesHard.Length];
        for(int i = 0; i < startPos.Length; i++)
            startPos[i] = ClothesHard[i].transform.position;

        Randomizer.Randomize(startPos);

        for(int i = 0; i < startPos.Length; i++)
            ClothesHard[i].transform.position = startPos[i];

    }

    void InitEasy()
    {
        HardContent.SetActive(false);
        EasyContent.SetActive(true);

        control.OnDrop += OnDrop;

        startPos = new Vector3[ClothesEasy.Length];
        for(int i = 0; i < startPos.Length; i++)
            startPos[i] = ClothesEasy[i].transform.position;

        Randomizer.Randomize(startPos);

        for(int i = 0; i < startPos.Length; i++)
            ClothesEasy[i].transform.position = startPos[i];
    }

    void OnDrop(GameObject go)
    {
        for(int i = 0; i < startPos.Length; i++)
        {
            if(Closet.bounds.Contains(ClothesEasy[i].transform.position))
            {
                if(ClothesEasy[i].name.Equals("Ok"))
                {
                    Win();
                    ClothesEasy[i].SetActive(false);
                }
                else
                {
                    ResetLevel();
                }
                ImportantAction();
            }
            ClothesEasy[i].transform.position = startPos[i];
        }
    }


    public void CheckHard()
    {
        if(Box.bounds.Contains(ClothesHard[0].transform.position))
            Win();
        else
            ResetLevel();
    }

    void Win()
    {
        for(int i = 0; i < OnCompleteKeys.Length; i++)
            DataManager.AddProgressKey(OnCompleteKeys[i], 1);

        SceneLoader.LoadScene(BaseScene);
    }

    void ResetLevel()
    {
        SceneLoader.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

}
