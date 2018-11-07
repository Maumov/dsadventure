using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTags : BaseGame
{
    [Header("Store")]
    public GameObject EasyNumbersParent;
    public GameObject HardNumbersParent;

    public GameObject[] EasyNumbers;
    public GameObject[] HardNumbers;
    Vector3[] numbersPos;

    public TextMesh Cokies;
    public TextMesh Total;

    public LayerMask DropLayer;
    Ray ray;
    RaycastHit hit;
    DragAndDrop control;

    int targetNumber;
    int max;

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
            max = Random.Range(10, 21);
        else
            max = Random.Range(5, 10);

        targetNumber = Random.Range(2, max);
        Total.text = max.ToString();
        Cokies.text = (max - targetNumber).ToString();
        tutorial.SetValues(new string[] { max.ToString(), (max - targetNumber).ToString() });
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
