using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetShopShape : BaseGame
{
    [Header("Pet Shop")]
    public GameObject EasyContent;
    public GameObject HardContent;

    DragAndDrop control;

    [Header("Hard")]
    public List<PetGroup> Groups = new List<PetGroup>();
    public float GroupDistance = 1.2f;
    public int GroupsSize = 3;
    public Transform[] PetsHard;
    public Collider[] ContainerHard;
    public int minPets;
    protected int allHardPets;

    [Header("Easy")]
    public Transform[] Pets;
    public Collider[] Container;
    public string[] Options = new string[] { "Cat", "Dog" };
    List<GameObject> content = new List<GameObject>();

    protected override void Initialize()
    {
        if(DataManager.IsHardGame)
        {
            EasyContent.SetActive(false);
            HardContent.SetActive(true);
            control = HardContent.GetComponent<DragAndDrop>();

            allHardPets = Random.Range(minPets, PetsHard.Length);
            for(int i = allHardPets; i < PetsHard.Length; i++)
            {
                PetsHard[i].gameObject.SetActive(false);
            }
        }
        else
        {
            EasyContent.SetActive(true);
            HardContent.SetActive(false);
            control = EasyContent.GetComponent<DragAndDrop>();
        }

        control.OnDrop += ImportantActionHard;
    }

    public override void SetControl(bool sw)
    {
        base.SetControl(sw);
        control.Active = sw;
    }

    public void ImportantActionEasy()
    {
        ImportantAction();
    }

    void ImportantActionHard(GameObject go)
    {
        for(int i = 0; i < Pets.Length; i++)
        {
            if(go.name != Pets[i].name && Vector3.SqrMagnitude(go.transform.position - Pets[i].position) < GroupDistance)
            {
                ImportantAction();
                return;
            }
        }
    }

    public void Check()
    {
        if(DataManager.IsNAGame)
        {
            SetControl(false);
            CompleteButton.SetActive(false);
            NAEnd();
            return;
        }
        if(DataManager.IsHardGame)
        {
            CheckHard();
        }
        else
        {
            CheckEasy();
        }

    }

    protected virtual void CheckHard()
    {
        SetControl(false);
        CompleteButton.SetActive(false);

        int currentGroup;
        int grouped = 0;
        int leftOvers = 0;
        for(int i = 0; i < ContainerHard.Length; i++)
        {
            currentGroup = 0;
            for(int j = 0; j < allHardPets; j++)
            {
                if(ContainerHard[i].bounds.Contains(PetsHard[j].position))
                {
                    currentGroup++;
                    grouped++;
                }
            }
            if(currentGroup > GroupsSize)
            {
                ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
                return;
            }

            if(currentGroup < GroupsSize)
                leftOvers += currentGroup;
        }

        if(allHardPets - grouped > GroupsSize - 1)
        {
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
            return;
        }

        if(allHardPets - grouped + leftOvers > GroupsSize - 1)
        {
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
            return;
        }

        ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
    }

    protected void CheckEasy()
    {
        SetControl(false);
        CompleteButton.SetActive(false);
        int total = 0, each = 0;

        for(int i = 0; i < Container.Length; i++)
        {
            each = 0;
            for(int j = 0; j < Pets.Length; j++)
            {
                if(Container[i].bounds.Contains(Pets[j].position))
                {
                    total++;
                    each++;
                }
            }
            if(each == 0)
            {
                ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
                return;
            }
        }

        if(total != Pets.Length)
        {
            ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
            return;
        }

        string checking = string.Empty;

        for(int i = 0; i < Container.Length; i++)
        {
            content.Clear();
            for(int j = 0; j < Pets.Length; j++)
            {
                if(Container[i].bounds.Contains(Pets[j].position))
                    content.Add(Pets[j].gameObject);
            }

            for(int j = 0; j < Options.Length; j++)
            {
                if(content[0].name.Contains(Options[j]))
                    checking = Options[j];
            }

            for(int j = 0; j < content.Count; j++)
            {
                if(!content[j].name.Contains(checking))
                {
                    ConversationUI.ShowText(LevelKeyName + Easy + Wrong, ResetLevel);
                    return;
                }
            }
        }

        ConversationUI.ShowText(LevelKeyName + Easy + Fine, Win);
    }

    [System.Serializable]
    public class PetGroup
    {
        public List<Transform> Group = new List<Transform>();

        public bool Compare(PetGroup p)
        {
            if(p.Group.Count != Group.Count)
                return false;

            for(int i = 0; i < Group.Count; i++)
            {
                if(!p.Group[i].Equals(Group[i]))
                    return false;
            }

            return true;
        }
    }
}


