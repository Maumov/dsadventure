using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    public KeyConversation[] Conversations;
    PlayerController player;
    int current;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        ShowConversations();
    }

    public void ShowConversations()
    {
        for(int i = 0; i < Conversations.Length; i++)
        {
            if(DataManager.CheckProgressKey(Conversations[i].Keys.Key) == Conversations[i].Keys.Value)
            {
                player.ControlState = false;
                current = i;
                ConversationUI.ShowText(Conversations[i].Message, () =>
                {
                    if(Conversations[current].AutoComplete)
                        DataManager.AddProgressKey(Conversations[current].Keys.Key, true);

                    player.ControlState = true;

                    if(Conversations[current].OnFinish != null)
                        Conversations[current].OnFinish.Invoke();
                });
                return;
            }
        }
    }


    [System.Serializable]
    public struct KeyConversation
    {
        public KeyCheck Keys;
        public ConversationData Message;
        public bool AutoComplete;
        public UnityEvent OnFinish;
    }
}
