using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    public KeyConversation[] Conversations;
    public ConversationData[] WellDone;
    public ConversationData[] Fail;
    public ConversationData TryAgain;
    public ConversationData WellcomeBack;
    PlayerController player;
    RoomCinematics cinematics;
    int current;
    bool feedback;

    public string[] GameKeys;
    int[] gameValues;
    bool breakRoom;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        cinematics = FindObjectOfType<RoomCinematics>();

        if(DataManager.GetSelectedFile().GameDifficult == -1)
            Evaluate();
        ShowConversations();
    }

    public void ShowConversations()
    {
        for(int i = 0; i < Conversations.Length; i++)
        {
            if(DataManager.CheckProgressKey(Conversations[i].Keys.Key) == Conversations[i].Keys.Value)
            {
                player.ControlState = false;

                if(i > 1)
                {
                    FriendNpc.CurrentConversation = Conversations[i].Message.Name;
                    cinematics.SetStaticFriend();
                }

                if(!feedback && i > 1)
                {
                    feedback = true;
                    if(SceneLoader.LastScene.Contains("Game"))
                    {
                        if(BaseGame.Quit)
                        {
                            BaseGame.Quit = false;
                            ConversationUI.ShowText(TryAgain, ShowConversations);
                            return;
                        }
                        else
                        {
                            int sw = -1;
                            DataManager.ProgressKeyValue(Conversations[i - 1].Keys.Key, out sw);
                            if(sw == 2)
                                ConversationUI.ShowText(WellDone[i - 1], ShowConversations);
                            else
                                ConversationUI.ShowText(Fail[i - 1], ShowConversations);
                            return;
                        }
                    }
                    else if(i > 0)
                    {
                        ConversationUI.ShowText(WellcomeBack, ShowConversations);
                        return;
                    }
                }

                current = i;

                if(i == 0 || i == 1)
                {
                    cinematics.CallEvent(i, Conversations[i], player);
                    return;
                }

                ConversationUI.ShowText(Conversations[i].Message, () =>
                {
                    if(Conversations[current].AutoComplete)
                        DataManager.AddProgressKey(Conversations[current].Keys.Key, 1);

                    player.ControlState = true;

                    if(Conversations[current].OnFinish != null)
                        Conversations[current].OnFinish.Invoke();
                });
                return;
            }
        }
    }

    void Evaluate()
    {
        gameValues = new int[GameKeys.Length];
        for(int i = 0; i < GameKeys.Length; i++)
            DataManager.ProgressKeyValue(GameKeys[i], out gameValues[i]);

        int temp;
        int last;

        #region First Rule

        temp = 0;
        last = -100;
        for(int i = 0; i < gameValues.Length; i++)
        {
            if(gameValues[i] == 2)
                temp++;
        }

        if(temp > 2)
        {
            DataManager.SetGameDifficult(2);
            breakRoom = true;
            return;
        }

        #endregion

        #region Second Rule

        temp = 0;
        last = -100;
        for(int i = 0; i < gameValues.Length; i++)
        {
            if(gameValues[i] == 1 && last == 1)
            {
                DataManager.SetGameDifficult(1);
                breakRoom = true;
                return;
            }

            last = gameValues[i];
        }

        #endregion

        #region Third Rule

        temp = 0;
        last = -100;
        for(int i = 0; i < gameValues.Length; i++)
        {
            if(gameValues[i] == 0 && last == 0)
            {
                DataManager.SetGameDifficult(0);
                breakRoom = true;
                return;
            }
            last = gameValues[i];
        }

        #endregion

        #region Fourth Rule

        temp = 0;
        last = 0;
        for(int i = 0; i < gameValues.Length; i++)
        {
            if(gameValues[i] == 1)
                temp++;
            if(gameValues[i] == 0)
                last++;
        }

        if(temp + last > 2)
        {
            if(last > temp)
                DataManager.SetGameDifficult(0);
            else
                DataManager.SetGameDifficult(1);
            breakRoom = true;
            return;
        }

        #endregion



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
