using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilesUI : GenericMenu
{
    public FileModelButton Model;

    void Start()
    {
        SetOptions();
    }

    void SetOptions()
    {
        FileData[] files = DataManager.GetAllFiles();

        FileModelButton current;
        for(int i = 0; i < files.Length; i++)
        {
            current = Instantiate(Model);
            current.transform.SetParent(Model.transform.parent);
            current.transform.localPosition = Vector3.zero;
            current.transform.localScale = Vector3.one;
            current.transform.SetAsLastSibling();
            current.Set(files[i], i);
        }
        Model.gameObject.SetActive(false);
    }
}
