using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarDatabase : MonoBehaviour
{
    public GameObject[] Model;

    public static GameObject[] ModelList
    {
        get
        {
            if(instance == null)
                instance = FindObjectOfType<AvatarDatabase>();
            return instance.Model;
        }
    }

    static AvatarDatabase instance;

    void Awake()
    {
        instance = this;
    }

    public static void Delete()
    {
        instance.Model = null;
        Destroy(instance.gameObject);
    }
}
