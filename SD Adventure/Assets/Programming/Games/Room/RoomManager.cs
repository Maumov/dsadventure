using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    public KeyConversation[] Conversations;
    public ConversationData WellDoneText;
    public ConversationData FailText;
    PlayerController player;
    int current;
    bool feedback;

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
                if(!feedback && SceneLoader.LastScene.Contains("Game") && i > 2)
                {
                    bool sw = false;
                    DataManager.ProgressKeyValue(Conversations[i - 1].Keys.Key, out sw);
                    if(sw)
                        ConversationUI.ShowText(WellDoneText, ShowConversations);
                    else
                        ConversationUI.ShowText(FailText, ShowConversations);
                    feedback = true;
                    return;
                }

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
