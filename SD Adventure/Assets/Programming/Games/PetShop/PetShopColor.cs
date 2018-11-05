using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetShopColor : PetShopShape
{

    public Transform PetsParent;
    protected override void Initialize()
    {
        base.Initialize();
        if(DataManager.IsHardGame)
            PetsParent.position += Vector3.forward * -1.8f;
    }

    protected override void CheckHard()
    {
        Groups.Clear();
        for(int i = 0; i < Pets.Length; i++)
        {
            Groups.Add(new PetGroup());
            for(int j = 0; j < Pets.Length; j++)
            {
                if(Vector3.SqrMagnitude(Pets[i].position - Pets[j].position) < GroupDistance)
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
            if(Groups[i].Group.Count > GroupsSize)
            {
                ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
                return;
            }

            if(Groups[i].Group.Count < GroupsSize)
                leftovers += Groups[i].Group.Count;
        }

        if(leftovers > GroupsSize - 1)
        {
            ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
            return;
        }


        string checking = string.Empty;

        for(int i = 0; i < Groups.Count; i++)
        {
            if(Groups[i].Group.Count != GroupsSize)
                continue;

            for(int j = 0; j < Options.Length; j++)
            {
                if(Groups[i].Group[0].name.Contains(Options[j]))
                    checking = Options[j];
            }

            for(int j = 0; j < Groups[i].Group.Count; j++)
            {
                if(!Groups[i].Group[j].name.Contains(checking))
                {
                    ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
                    return;
                }
            }
        }

        ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
    }
}
