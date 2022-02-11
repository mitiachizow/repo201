using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;

public class Apartment : Construction
{
    public Apartment(ConstructionPrefab prefab, ConstructionLVL сonstructionLvl, UnityEngine.Transform parent, Vector3 positon, GridLayout grid) : base(prefab, сonstructionLvl, parent, positon, grid) { }

    public override void ShowInfo()
    {
        GameObject.Find("Canvas Controller").GetComponent<CanvasController>().ForceChangeCanvas(infoPlane: true, addConstruction:true);
        //GameObject.Find("Info Plane").GetComponent<InfoPlane>().Size = InfoPlane.PlaneSize.Small;
        Debug.Log("Show Info");
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
