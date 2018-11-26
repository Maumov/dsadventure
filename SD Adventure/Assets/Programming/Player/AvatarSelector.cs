using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelector : MonoBehaviour
{
    [HideInInspector]
    [System.NonSerialized]
    public GameObject SelectedAvatar;

    void Awake()
    {
        SelectedAvatar = Instantiate(AvatarDatabase.ModelList[DataManager.GetSelectedFile().AvatarId]);
        SelectedAvatar.transform.SetParent(transform);
        SelectedAvatar.transform.localPosition = Vector3.zero;
        SelectedAvatar.transform.localScale = Vector3.one;
        SelectedAvatar.transform.localEulerAngles = Vector3.zero;
        GetComponent<PlayerController>().Anim = SelectedAvatar.GetComponent<Animator>();
        AvatarDatabase.Delete();
    }
}
