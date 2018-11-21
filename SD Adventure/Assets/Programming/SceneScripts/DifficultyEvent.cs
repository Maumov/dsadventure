using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DifficultyEvent : MonoBehaviour
{

    public UnityEvent OnEasy;
    public UnityEvent OnHard;

    private void Start()
    {
        if(DataManager.IsHardGame)
        {
            if(OnHard != null)
                OnHard.Invoke();
        }
        else
        {
            if(OnEasy != null)
                OnEasy.Invoke();
        }
    }
}
