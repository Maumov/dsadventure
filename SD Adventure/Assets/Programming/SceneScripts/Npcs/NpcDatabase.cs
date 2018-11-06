using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName ="Npc Database", menuName ="Custom/Create Npc Database", order = 0)]
public class NpcDatabase : ScriptableObject
{
    public NpcText[] AllTexts;
    //public NpcText

    public string GetConversation(string npc)
    {
        string str = "NpcDefaultText";

        for(int i = 0; i < AllTexts.Length; i++)
        {
            if(DataManager.CheckProgressKey(AllTexts[i].Key.Key) == AllTexts[i].Key.Value)
            {
                if(npc.Equals(AllTexts[i].MainNpc))
                    return AllTexts[i].MainConversation;
                else
                    return AllTexts[i].ExtraConversation;
            }
        }

        return str;
    }
}

[System.Serializable]
public class NpcText
{
    public KeyCheck Key;
    public string MainNpc;
    public string MainConversation;
    public string ExtraConversation;
}