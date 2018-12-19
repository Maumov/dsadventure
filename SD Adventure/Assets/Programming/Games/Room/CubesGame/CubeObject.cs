using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObject : MonoBehaviour, IObjectIDController
{
    public GameObjectArray[] Cubes;
    protected static int[] Index;

	public string id;
	public string objectId{
		get{
			return id;
		}
		set{
			id = value;
		}
	}

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
		objectId = id+"";
    }

    private void OnDestroy()
    {
        Index = null;
    }
}
