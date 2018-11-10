using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumber : MonoBehaviour
{
    public int Min;
    public int Max;
    public string Prefix;
    TextMesh text;

    private void Start()
    {
        text = GetComponent<TextMesh>();
        text.text = Prefix + Random.Range(Min, Max);
    }
}
