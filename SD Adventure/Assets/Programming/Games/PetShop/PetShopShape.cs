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
    public Transform[] Pets;
    public List<PetGroup> Groups = new List<PetGroup>();

    [Header("Easy")]
    public Collider[] Container;
    List<GameObject> leftContent = new List<GameObject>();
    List<GameObject> rightContent = new List<GameObject>();

    protected override void Initialize()
    {
        if(DataManager.IsHardGame)
        {
            EasyContent.SetActive(false);
            HardContent.SetActive(true);
            control = HardContent.GetComponent<DragAndDrop>();
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
            if(go.name != Pets[i].name && Vector3.SqrMagnitude(go.transform.position - Pets[i].position) < 1.2f)
                ImportantAction();
        }
    }

    public void Check()
    {
        Groups.Clear();
        if(DataManager.IsHardGame)
        {
            for(int i = 0; i < Pets.Length; i++)
            {
                Groups.Add(new PetGroup());
                for(int j = 0; j < Pets.Length; j++)
                {
                    if(Vector3.SqrMagnitude(Pets[i].position - Pets[j].position) < 1.2f)
                    {
                        Groups[Groups.Count - 1].Group.Add(Pets[j]);
                    }
                }
                if(Groups[Groups.Count - 1].Group.Count == 0)
                    Groups.RemoveAt(Groups.Count - 1);
                else
                    Groups[Groups.Count - 1].Group.Sort(delegate (Transform x, Transform y)
                    {
                        return x.name.CompareTo(y.name);
                    });
            }

            for(int i = 0; i < Groups.Count; i++)
            {
                for(int j = 0; j < Groups.Count; j++)
                {
                    if(i != j && Groups[i].Compare(Groups[j]))
                    {
                        Groups.RemoveAt(j);
                        j--;
                    }
                }
            }


            int leftovers = 0;
            for(int i = 0; i < Groups.Count; i++)
            {
                if(Groups[i].Group.Count > 3)
                {
                    Debug.Log("Group too big");
                    return;
                }

                if(Groups[i].Group.Count < 3)
                    leftovers += Groups[i].Group.Count;
            }

            if(leftovers > 2)
            {
                Debug.Log("Incomplete groups");
                return;
            }

            Win();
        }
        else
        {
            leftContent.Clear();
            rightContent.Clear();
            for(int i = 0; i < Pets.Length; i++)
            {
                if(Container[0].bounds.Contains(Pets[i].transform.position))
                {
                    leftContent.Add(Pets[i].gameObject);
                }

                if(Container[1].bounds.Contains(Pets[i].transform.position))
                {
                    rightContent.Add(Pets[i].gameObject);
                }
            }

            if(leftContent.Count + rightContent.Count != Pets.Length)
            {
                Debug.Log("Unclasified pets");
                return;
            }

            if(leftContent.Count == 0)
            {
                Debug.Log("No content on left content");
                return;
            }

            if(rightContent.Count == 0)
            {
                Debug.Log("No content on right content");
                return;
            }

            string checking;

            if(leftContent[0].name.Contains("Cat"))
                checking = "Cat";
            else
                checking = "Dog";

            for(int i = 0; i < leftContent.Count; i++)
            {
                if(!leftContent[i].name.Contains(checking))
                {
                    Debug.Log("No match on left content");
                    return;
                }
            }

            if(rightContent[0].name.Contains("Cat"))
                checking = "Cat";
            else
                checking = "Dog";

            for(int i = 0; i < rightContent.Count; i++)
            {
                if(!rightContent[i].name.Contains(checking))
                {
                    Debug.Log("No match on right content");
                    return;
                }
            }

            Win();
        }

    }

    void Win()
    {
        DataManager.AddProgressKey("PetShop-1", 1);
        SceneLoader.LoadScene(BaseScene);
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


