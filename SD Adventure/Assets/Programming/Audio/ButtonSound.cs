using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class ButtonSound : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SfxManager.Play(SFXType.Button);
    }
}
