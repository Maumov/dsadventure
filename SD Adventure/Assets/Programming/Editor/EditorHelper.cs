using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorHelper : Editor
{
    [MenuItem("Custom/Clear PayerPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        DataManager.Load();
        Debug.Log("Player Prefs Deleted");
    }


    [MenuItem("Custom/Change difficulty")]
    public static void ChangeDiffuclty()
    {
        if(DataManager.GetAllFiles().Length == 0)
        {
            FileData newFile = new FileData("Helper", 0);
            DataManager.AddFile(newFile);
            DataManager.SetSelectedFile(0);
        }

        DataManager.GetSelectedFile().GameDifficult++;
        if(DataManager.GetSelectedFile().GameDifficult > 2)
            DataManager.GetSelectedFile().GameDifficult = 0;

        DataManager.Save();
        Debug.Log("Game[0] difficulty: " + DataManager.GetSelectedFile().GameDifficult);
    }

}
