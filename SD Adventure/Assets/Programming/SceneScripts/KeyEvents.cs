using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyEvents : MonoBehaviour
{
    public EventValidation[] Events;

    private void Start()
    {
        bool result;

        for(int i = 0; i < Events.Length; i++)
        {
            result = false;
            for(int j = 0; j < Events[i].Keys.Length; j++)
            {
                if(j == 0)
                    result = DataManager.CheckProgressKey(Events[i].Keys[j].Key) == Events[i].Keys[j].Value;
                else
                    result = DataManager.CheckProgressKey(Events[i].Keys[j].Key) == Events[i].Keys[j].Value && result;
            }
            if(result && Events[i].Call != null)
                Events[i].Call.Invoke();

        }
    }

    [System.Serializable]
    public struct EventValidation
    {
        public KeyCheck[] Keys;
        public UnityEvent Call;
    }

}
[System.Serializable]
public struct KeyCheck
{
    public string Key;
    public bool Value;
}
