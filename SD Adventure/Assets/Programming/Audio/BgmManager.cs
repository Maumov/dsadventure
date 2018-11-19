using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BgmManager : MonoBehaviour
{
    static BgmManager instance;
    static AudioMixer mixer;

    public AudioClip Clip;
    AudioSource source;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        if(instance != this)
        {
            if(instance.Clip != Clip)
            {
                instance.Clip = Clip;
                instance.Play();
            }

            Destroy(gameObject);
            return;
        }

        if(mixer == null)
            mixer = Resources.Load<AudioMixer>("AudioMixer");

        source = gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 0;
        source.playOnAwake = false;
        source.loop = true;

        source.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];

        Play();
    }


    void Play()
    {
        source.clip = Clip;
        source.Play();
    }
}
