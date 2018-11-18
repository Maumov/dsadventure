using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxManager : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioClip[] Clips;
    public SFXType Order;

    public static SfxManager Instance;
    AudioSource source;

    void Init()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 0;
        source.playOnAwake = false;
        source.loop = false;

        source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
    }

    public static void Play(SFXType clip)
    {
        Check();
        Instance.source.PlayOneShot(Instance.Clips[(int)clip]);
    }

    static void Check()
    {
        if(Instance == null)
        {
            Instance = Instantiate(Resources.Load<SfxManager>("Sfx Manager"));
            Instance.Init();
            DontDestroyOnLoad(Instance.gameObject);
        }
    }

}

public enum SFXType : byte
{
    Pick = 0,
    Button,
    Basket
}