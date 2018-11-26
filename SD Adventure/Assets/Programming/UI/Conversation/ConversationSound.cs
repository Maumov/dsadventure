using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationSound : MonoBehaviour
{
    public AudioClip[] Clips;
    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayConversation(string str)
    {
        for(int i = 0; i < Clips.Length; i++)
        {
            if(Clips[i].name.Equals(str))
            {
                source.Stop();
                source.clip = Clips[i];
                source.Play();
                return;
            }
        }
    }
}
