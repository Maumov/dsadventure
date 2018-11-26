using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangHelper : MonoBehaviour {

    public GameObject[] Clothes;
    public GameObject[] Hanger;
    public GameObject[] ClothesPiles;

    private void Start()
    {
        int r = Random.Range(5,7);

        for(int i = 0; i < Clothes.Length; i++)
        {
            Clothes[i].SetActive(false);
            Hanger[i].SetActive(false);
            ClothesPiles[i].SetActive(false);
        }

        for(int i = 0; i < r; i++)
        {
            Clothes[i].SetActive(true);
            Hanger[i].SetActive(true);
            ClothesPiles[i].SetActive(true);
        }
    }
}
