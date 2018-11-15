using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Domino : CubeObject
{
    public TextMesh[] BoxText;

    public override void Set(int id)
    {
        BoxText[id].text = Cubes[id].Objects[Index[id]].name;
        base.Set(id);
    }
}
