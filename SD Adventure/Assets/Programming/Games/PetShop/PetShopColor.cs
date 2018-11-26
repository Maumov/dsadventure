using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetShopColor : PetShopShape
{

    public Transform PetsParent;

    protected override void CheckHard()
    {
        SetControl(false);
        CompleteButton.SetActive(false);

        int currentGroup;
        int grouped = 0;
        int leftOvers = 0;
        string feature;

        for(int i = 0; i < ContainerHard.Length; i++)
        {
            currentGroup = 0;
            feature = string.Empty;
            for(int j = 0; j < allHardPets; j++)
            {
                if(ContainerHard[i].bounds.Contains(PetsHard[j].position))
                {
                    currentGroup++;
                    grouped++;

                    if(string.IsNullOrEmpty(feature))
                    {
                        for(int k = 0; k < Options.Length; k++)
                        {
                            if(PetsHard[j].name.Contains(Options[k]))
                                feature = Options[k];
                        }
                    }
                    if(!PetsHard[j].name.Contains(feature))
                    {
                        ConversationUI.ShowText(LevelKeyName + Hard + Wrong, ResetLevel);
                        return;
                    }
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
        InGameStars.Show(LevelPos);
        ConversationUI.ShowText(LevelKeyName + Hard + Fine, Win);
    }
}
