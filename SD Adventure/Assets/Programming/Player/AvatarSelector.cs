using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelector : MonoBehaviour
{
    [HideInInspector]
    [System.NonSerialized]
    public GameObject SelectedAvatar;

    void Start()
    {
        SelectedAvatar = Instantiate(AvatarDatabase.ModelList[DataManager.GetSelectedFile().AvatarId]);
        SelectedAvatar.transform.SetParent(transform);
        SelectedAvatar.transform.localPosition = Vector3.zero;
        SelectedAvatar.transform.localScale = Vector3.one;
        SelectedAvatar.transform.localEulerAngles = Vector3.zero + Vector3.right * -90;
        AvatarDatabase.Delete();
    }
}
