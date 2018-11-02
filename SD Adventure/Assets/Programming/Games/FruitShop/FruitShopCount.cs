using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitShopCount : BaseGame
{
    [Header("Fruit Shop")]
    public string[] OnCompleteKeys;
    public LayerMask DropLayer;
    DragAndDrop control;
    Ray ray;
    RaycastHit hit;
    int targetNumber = 5;

    [Header("Fruit Shop Easy")]
    public GameObject EasyContent;
    public GameObject[] Numbers;
    public GameObject[] EasyFruits;
    public Transform GoodPosition;
    Vector3[] startPos;


    [Header("Fruit Shop Hard")]
    public GameObject HardContent;
    public TextMesh RequiredFruits;
    public Collider FruitContainer;
    public GameObject[] HardFruits;
    Vector3 hardStartPos;

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    public override void StartGame()
    {
        base.StartGame();

        for(int i = 0; i < Numbers.Length; i++)
            Numbers[i].SetActive(true);
    }

    protected override void Initialize()
    {
        control = FindObjectOfType<DragAndDrop>();

        if(DataManager.IsHardGame)
            InitHard();
        else
            InitEasy();
    }

    void InitHard()
    {
        HardContent.SetActive(true);
        EasyContent.SetActive(false);

        control.OnDrag += HardDrag;
        control.OnDrop += HardDrop;

        targetNumber = Random.Range(10, 21);
        RequiredFruits.text = targetNumber.ToString();
    }

    void HardDrag(GameObject go)
    {
        hardStartPos = go.transform.position;
    }

    void HardDrop(GameObject go)
    {
        ray = control.Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100, DropLayer.value))
        {
            go.transform.position = hit.point;
            if(hit.transform.name.Equals("Target"))
                ImportantAction();
        }
        else
            go.transform.position = hardStartPos;
    }

    public void CheckHard()
    {
        int lemons = 0;
        for(int i = 0; i < HardFruits.Length; i++)
        {
            if(FruitContainer.bounds.Contains(HardFruits[i].transform.position))
            {
                if(i < 9)
                {
                    Debug.Log("Fruta incorrecta");
                    return;
                }
                else
                    lemons++;
            }
        }
        if(lemons != targetNumber)
        {
            Debug.Log("Cantidad incorrecta");
            return;
        }

        Win();
    }

    void InitEasy()
    {
        HardContent.SetActive(false);
        EasyContent.SetActive(true);

        CompleteButton = null;
        startPos = new Vector3[Numbers.Length];
        for(int i = 0; i < startPos.Length; i++)
        {
            startPos[i] = Numbers[i].transform.position;
            Numbers[i].SetActive(false);
        }

        control.OnDrop += EasyDrop;

        targetNumber = Random.Range(1, 10);

        Randomizer.Randomize(EasyFruits);
        for(int i = 0; i < EasyFruits.Length; i++)
        {
            if(i > targetNumber - 1)
                EasyFruits[i].SetActive(false);
        }

    }

    void EasyDrop(GameObject go)
    {
        ray = control.Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100, DropLayer.value))
        {
            ImportantAction();
            if(go.name.Equals(targetNumber.ToString()))
            {
                Win();
                SetControl(false);
                go.transform.SetParent(GoodPosition);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = GoodPosition.rotation;
                go.transform.localScale = Vector3.one;
                return;
            }
        }

        for(int i = 0; i < startPos.Length; i++)
            Numbers[i].transform.position = startPos[i];
    }


    void Win()
    {
        for(int i = 0; i < OnCompleteKeys.Length; i++)
            DataManager.AddProgressKey(OnCompleteKeys[i], 1);

        SceneLoader.LoadScene(BaseScene);
    }
}
