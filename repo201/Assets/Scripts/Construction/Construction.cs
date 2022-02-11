using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Construction : IInfo
{
    protected GameObject gameObject;
    protected Vector3 Size;
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
    public Vector3 Pivot { get; private set; }
    public string Name
    {
        get
        {
            return gameObject.name;
        }
    }
    public abstract void ShowInfo();
    public abstract void Upgrade();

    public void Build()
    {
        gameObject.tag = "Building";
        gameObject.name = gameObject.name + Random.Range(1,100); //поменять
    }

    protected Construction(ConstructionPrefab prefab, ConstructionLVL сonstructionLvl, UnityEngine.Transform parent, Vector3 positon, GridLayout grid)
    {
        this.Size = prefab.Size;
        this.сonstructionLVL = сonstructionLvl;

        gameObject = new GameObject();

        gameObject.AddComponent<MeshRenderer>();

        gameObject.AddComponent<MeshFilter>();
        gameObject.GetComponent<MeshFilter>().sharedMesh = prefab.Mesh;

        gameObject.AddComponent<BoxCollider>();

        gameObject.transform.position = grid.CellToLocal(grid.LocalToCell(new Vector3(positon.x, prefab.Size.y / 2, positon.y)));
        gameObject.transform.parent = parent;
        gameObject.tag = "Building Preview";
        gameObject.name = prefab.Name;
        gameObject.transform.localScale = prefab.Size;
        Pivot = gameObject.transform.position;
        Debug.Log(prefab.Size);

        //gameObject.GetComponent<MeshFilter>().transform.localScale = new Vector3(Mathf.Ceil(prefab.Size.x / SceneBehavior.SceneParams.cellSize), Mathf.Ceil(prefab.Size.y / SceneBehavior.SceneParams.cellSize), Mathf.Ceil(prefab.Size.z / SceneBehavior.SceneParams.cellSize));

        gameObject.AddComponent<Outline>();
        gameObject.GetComponent<Outline>().OutlineWidth = 10;
        gameObject.GetComponent<Outline>().enabled = false;

        gameObject.AddComponent<BuildingTransform>();
        gameObject.GetComponent<BuildingTransform>().enabled = false;

        prefab.RandomBuilding();
    }

    public void KillMe()
    {
        GameObject.Destroy(gameObject);
        Debug.Log("Construction is dead");
    }


}
