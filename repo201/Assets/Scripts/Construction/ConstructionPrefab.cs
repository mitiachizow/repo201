using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructionBehaviour;


[CreateAssetMenu(fileName = "Construction", menuName = "Construction")]
public class ConstructionPrefab : ScriptableObject
{
    [SerializeField]
    private Vector3Int cellCountSize;
    [SerializeField]
    private ConstructionType constructionType;
    [SerializeField]
    private Mesh[] mesh;
    [SerializeField]
    private Vector3Int meshSize;
    [SerializeField]
    private new string name;

    public string Name
    {
        get { return name; }
    }

    public Vector3Int CellCountSize
    {
        get { return cellCountSize; }
    }

    public ConstructionType ConstructionType
    {
        get { return constructionType; }
    }

    public Mesh Mesh
    {
        get { return mesh[Random.Range(0,mesh.Length)]; }
    }

    public Vector3Int MeshSize
    {
        get { return meshSize; }
    }
}
