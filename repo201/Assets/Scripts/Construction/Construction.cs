using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Construction : IInfo
{
    protected GameObject gameObject;
    protected Vector3Int cellCountSize;
    protected ConstructionLVL сonstructionLVL;
    public string Tag
    {
        get
        {
            return gameObject.tag;
        }
        set
        {
            gameObject.tag = value;
        }
    }
    public Vector3Int Pivot { get; private set; }
    public string Name
    {
        get
        {
            return gameObject.name;
        }
    }
    public abstract void ShowInfo();
    public abstract void Upgrade();

    protected Construction(ConstructionPrefab prefab, ConstructionLVL сonstructionLvl, UnityEngine.Transform parent, Vector3 positon, GridLayout grid)
    {
        this.cellCountSize = prefab.CellCountSize;
        this.сonstructionLVL = сonstructionLvl;

        gameObject = new GameObject();

        gameObject.transform.position = grid.CellToLocal(grid.LocalToCell(new Vector3(positon.x, 1f, positon.y)));
        gameObject.transform.parent = parent;
        gameObject.transform.localScale = prefab.MeshSize * SceneBehavior.SceneParams.cellSize;
        gameObject.tag = "Building Preview";
        gameObject.name = prefab.Name;

        gameObject.AddComponent<BoxCollider>();
        gameObject.GetComponent<BoxCollider>().size = new Vector3Int(1,1,1);

        gameObject.AddComponent<MeshRenderer>();

        gameObject.AddComponent<MeshFilter>();
        gameObject.GetComponent<MeshFilter>().sharedMesh = prefab.Mesh;
        gameObject.GetComponent<MeshFilter>().transform.localScale = prefab.MeshSize * SceneBehavior.SceneParams.cellSize;

        gameObject.AddComponent<Outline>();
        gameObject.GetComponent<Outline>().OutlineWidth = 10;
        gameObject.GetComponent<Outline>().enabled = false;

        gameObject.AddComponent<BuildingTransform>();
        gameObject.GetComponent<BuildingTransform>().enabled = false;
    }

    public void KillMe()
    {
        GameObject.Destroy(gameObject);
        Debug.Log("Construction is dead");
    }

    //public abstract void KillMe()
    //{
    //    GameObject.Destroy(gameObject);
    //    Debug.Log("I'm dead now");
    //}

    //~Construction()
    //{
    //    //GameObject.Destroy(gameObject);
    //    Debug.Log("Construction is dead");
    //}


}
