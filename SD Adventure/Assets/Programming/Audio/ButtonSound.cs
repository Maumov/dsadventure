using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class ButtonSound : MonoBehaviour, IPointerDownHandler
{
    static AudioClip bip;
    static AudioMixer mixer;
    AudioSource source;

    void Start()
    {
        if(bip == null)
            bip = Resources.Load<AudioClip>("Button");

        if(mixer == null)
            mixer = Resources.Load<AudioMixer>("AudioMixer");

        source = gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 0;
        source.playOnAwake = false;
        source.loop = false;

        source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        source.PlayOneShot(bip);
    }
}
