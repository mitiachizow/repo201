using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apartment : Construction
{
    public Apartment(ConstructionPrefab prefab, ConstructionLVL сonstructionLvl, UnityEngine.Transform parent, Vector3 positon, GridLayout grid) : base(prefab, сonstructionLvl, parent, positon, grid) { }

    public override void ShowInfo()
    {
        Debug.Log("Show info");
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    ~Apartment()
    {
        Debug.Log("Apartment is dead");
    }

                

}
