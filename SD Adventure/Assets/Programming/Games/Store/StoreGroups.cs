using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreGroups : BaseGame
{
    [Header("Store")]

    public GameObject EasyNumbersParent;
    public GameObject HardNumbersParent;

    public GameObject BottlesEasyParent;
    public GameObject BottlesHardParent;

    public GameObject[] EasyNumbers;
    public GameObject[] HardNumbers;
    Vector3[] numbersPos;

    public LayerMask DropLayer;
    Ray ray;
    RaycastHit hit;
    DragAndDrop control;

    int targetNumber;

    public GameObjectArray[] BottlesEasy;
    public GameObjectArray[] BottlesHard;

    protected override void Initialize()
    {
        EasyNumbersParent.SetActive(false);
        HardNumbersParent.SetActive(false);

        control = FindObjectOfType<DragAndDrop>();
        control.OnDrop += Drop;

        numbersPos = new Vector3[EasyNumbers.Length + HardNumbers.Length];

        for(int i = 0; i < EasyNumbers.Length; i++)
            numbersPos[i] = EasyNumbers[i].transform.position;

        for(int i = EasyNumbers.Length; i < EasyNumbers.Length + HardNumbers.Length; i++)
            numbersPos[i] = HardNumbers[i - EasyNumbers.Length].transform.position;

        if(DataManager.IsHardGame)
        {
            BottlesEasyParent.SetActive(false);
            BottlesHardParent.SetActive(true);

            targetNumber = Random.Range(3, 21);
            for(int i = 0; i < BottlesHard.Length; i++)
            {
                //Randomizer.Randomize(BottlesEasy[i].Objects);
                for(int j = 0; j < BottlesHard[i].Objects.Length; j++)
                    BottlesHard[i].Objects[j].SetActive(false);

            }

            int a = 0, b = 0, c = 0;

            for(int i = 0; i < targetNumber; i++)
            {
                if(i % 3 == 0)
                {
                    BottlesHard[0].Objects[a].SetActive(true);
                    a++;
                }
                else if(i % 3 == 1)
                {
                    BottlesHard[1].Objects[b].SetActive(true);
                    b++;
                }
                else
                {
                    BottlesHard[2].Objects[c].SetActive(true);
                    c++;
                }

            }
        }
        else
        {
            BottlesEasyParent.SetActive(true);
            BottlesHardParent.SetActive(false);

            targetNumber = Random.Range(2, 10);
            for(int i = 0; i < BottlesEasy.Length; i++)
            {
                //Randomizer.Randomize(BottlesEasy[i].Objects);
                for(int j = 0; j < BottlesEasy[i].Objects.Length; j++)
                    BottlesEasy[i].Objects[j].SetActive(false);

            }

            for(int i = 0; i < targetNumber; i++)
            {
                if(i % 2 == 0)
                    BottlesEasy[0].Objects[i].SetActive(true);
                else
                    BottlesEasy[1].Objects[i].SetActive(true);
            }
        }
    }

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    public override void StartGame()
    {
        base.StartGame();
        EasyNumbersParent.SetActive(true);
        if(DataManager.IsHardGame)
            HardNumbersParent.SetActive(true);
    }

    void Drop(GameObject go)
    {

        ray = control.Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100, DropLayer.value))
        {
            ImportantAction();
            SetControl(false);
            if(go.name.Equals(targetNumber.ToString()))
            {
                go.transform.position = hit.transform.position + go.transform.forward * -0.01f;
                if(DataManager.IsHardGame)
                    ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
                else
                    ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
            }
            else
            {
                go.transform.position = numbersPos[go.transform.GetSiblingIndex() + go.transform.parent.GetSiblingIndex() * EasyNumbers.Length];
                if(DataManager.IsHardGame)
                    ConversationUI.ShowText(LevelKeyName + Hard + Wrong, () => SetControl(true));
                else
                    ConversationUI.ShowText(LevelKeyName + Easy + Wrong, () => SetControl(true));
            }
        }
        else
            go.transform.position = numbersPos[go.transform.GetSiblingIndex() + go.transform.parent.GetSiblingIndex() * EasyNumbers.Length];
    }


}
[System.Serializable]
public class GameObjectArray
{
    public GameObject[] Objects;
}
