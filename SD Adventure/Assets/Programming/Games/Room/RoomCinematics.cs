using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCinematics : MonoBehaviour
{

    public GameObject Door;
    public SpriteRenderer Fade;
    Color fadeColor;
    RoomManager.KeyConversation conversation;
    PlayerController player;

    [Header("Mom")]
    public Animator Mom;
    public Transform MomFinalPos;
    Vector3 momExitPos;


    [Header("Friend")]
    public FriendNpc Friend;
    public Animator FriendAnim;
    public Transform FriendFinalPos;
    public Transform FriendStaticPos;

    private void Start()
    {
        fadeColor = Fade.color;
        fadeColor.a = 0;
        Fade.color = fadeColor;
        Friend.Hide();
    }

    public void CallEvent(int id, RoomManager.KeyConversation c, PlayerController p)
    {
        conversation = c;
        player = p;
        if(id == 0)
            StartCoroutine(MomSequence());
        if(id == 1)
            StartCoroutine(FriendSequence());
    }

    IEnumerator MomSequence()
    {
        yield return new WaitForSeconds(0.5f);
        Mom.SetFloat("Movement", 2.4f);
        LeanTween.move(Mom.gameObject, MomFinalPos.transform.position, 2);
        yield return new WaitForSeconds(0.6f);

        LeanTween.rotateLocal(Mom.gameObject, new Vector3(0, 320, 0), 1.4f);
        LeanTween.rotateLocal(Door, new Vector3(-90, 100, 90), 0.5f);

        yield return new WaitForSeconds(1.4f);
        Mom.SetFloat("Movement", 0f);
        Mom.CrossFade("Talk", 0.25f);

        ConversationUI.ShowText(conversation.Message, () =>
        {
            StartCoroutine(FinishMom());
        });
    }

    IEnumerator FinishMom()
    {
        float t = 0;

        while(t < 1)
        {
            t += Time.deltaTime * 2;
            fadeColor.a = t;
            Fade.color = fadeColor;
            yield return null;
        }

        t = 1;
        fadeColor.a = t;
        Fade.color = fadeColor;

        Mom.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);


        while(t > 0)
        {
            t -= Time.deltaTime * 2;
            fadeColor.a = t;
            Fade.color = fadeColor;
            yield return null;
        }

        t = 0;
        fadeColor.a = t;
        Fade.color = fadeColor;

        if(conversation.AutoComplete)
            DataManager.AddProgressKey(conversation.Keys.Key, 1);

        player.ControlState = true;

        if(conversation.OnFinish != null)
            conversation.OnFinish.Invoke();
    }


    IEnumerator FriendSequence()
    {
        Door.transform.localEulerAngles = new Vector3(-90, 100, 90);
        FriendAnim.SetFloat("Movement", 2.4f);
        LeanTween.rotateLocal(Friend.gameObject, new Vector3(0, 320, 0), 2f);
        LeanTween.move(Friend.gameObject, FriendFinalPos.transform.position, 2);
        yield return new WaitForSeconds(2f);

        FriendAnim.SetFloat("Movement", 0f);
        FriendAnim.CrossFade("Talk", 0.25f);
        ConversationUI.ShowText(conversation.Message, () =>
        {
            StartCoroutine(FriendEnd());
        });
    }

    IEnumerator FriendEnd()
    {
        float t = 0;

        while(t < 1)
        {
            t += Time.deltaTime * 2;
            fadeColor.a = t;
            Fade.color = fadeColor;
            yield return null;
        }

        t = 1;
        fadeColor.a = t;
        Fade.color = fadeColor;

        SetStaticFriend();
        FriendNpc.CurrentConversation = conversation.Message.Name;
        Door.transform.localEulerAngles = new Vector3(-90, 0, 90);
        yield return new WaitForSeconds(0.5f);


        while(t > 0)
        {
            t -= Time.deltaTime * 2;
            fadeColor.a = t;
            Fade.color = fadeColor;
            yield return null;
        }

        t = 0;
        fadeColor.a = t;
        Fade.color = fadeColor;


        if(conversation.AutoComplete)
            DataManager.AddProgressKey(conversation.Keys.Key, 1);

        player.ControlState = true;

        if(conversation.OnFinish != null)
            conversation.OnFinish.Invoke();
    }

    public void SetStaticFriend()
    {
        Friend.transform.position = FriendStaticPos.position;
        Friend.transform.rotation = FriendStaticPos.rotation;
        Friend.Show();
    }
}
