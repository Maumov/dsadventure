using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObject : MonoBehaviour
{
    public GameObjectArray[] Cubes;
    protected static int[] Index;

    private void Start()
    {
        if(Index == null)
        {
            Index = new int[Cubes.Length];
        }
    }

    public virtual void Set(int id)
    {
        Transform current = Cubes[id].Objects[Index[id]].transform;
        current.SetParent(transform);
        current.localPosition = Vector3.zero;
        Index[id]++;
        name = id.ToString();
    }

    private void OnDestroy()
    {
        Index = null;
    }
}
