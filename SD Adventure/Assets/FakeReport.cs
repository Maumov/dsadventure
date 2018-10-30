using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeReport : MonoBehaviour
{
    [System.Serializable]
    public struct ReportText
    {
        public string Key;
        public string GameName;
    }

    public ReportText[] Keys;
    public string[] LevelsText;

    public UnityEngine.UI.Text T;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        int k = -2;
        T.text = string.Empty;
        for(int i = 0; i < Keys.Length; i++)
        {
            DataManager.ProgressKeyValue(Keys[i].Key, out k);
            k++;
            T.text += Keys[i].GameName + ": " + LevelsText[k] + "\n";
        }

        T.text += "Dificultad: " + LevelsText[DataManager.GetSelectedFile().GameDifficult + 1];
    }

}
