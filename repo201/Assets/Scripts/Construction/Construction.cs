using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConstructionBehaviour
{


    //Сделать класс синглтоном
    public class Construction
    {
        public int xSize, ySize,zSize;
        public Vector2Int pivot;
        public GameObject currentConstruction;


        MeshFilter currentBuildingMesh;
        BoxCollider currentBoxCollider;

        public Construction(string prefabToLoad)
        {
            currentConstruction =  MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Building Prefab"));

            xSize = 10;
            ySize = 10;
            zSize = 10;

            pivot = new Vector2Int((int)currentConstruction.transform.position.x, (int)currentConstruction.transform.position.z);

            currentBuildingMesh = currentConstruction.transform.Find("3D Model").GetComponent<MeshFilter>();

            currentBuildingMesh.sharedMesh = Resources.Load<MeshFilter>("Meshes/" + prefabToLoad).sharedMesh;

            currentBoxCollider = currentConstruction.transform.Find("3D Model").GetComponent<BoxCollider>();

            //currentConstruction.transform.localScale = new Vector3(currentConstruction.transform.localScale.x, currentConstruction.transform.localScale.y, currentConstruction.transform.localScale.z)* 2f;

            //currentBoxCollider.size = new Vector3(xSize, ySize, zSize) * ConstructionController.PointSize;

            currentConstruction.name = "Building Prefab: " + prefabToLoad;
        }

        public void ChangeType(string objectType)
        {
            switch (objectType)
            {
                case "Factory":
                    xSize = 10;
                    ySize = 10;
                    currentBuildingMesh.sharedMesh = Resources.Load<MeshFilter>("Meshes/Factory").sharedMesh;
                    //currentConstruction.transform.localScale = new Vector3(currentConstruction.transform.localScale.x, currentConstruction.transform.localScale.y, currentConstruction.transform.localScale.z) / 2f;
                    //currentBoxCollider.size = new Vector3(xSize,ySize,zSize) * ConstructionController.PointSize;//вот тут x,y,z перемножать на размер одного поинта
                    break;
                case "Apartment":
                    xSize = 10;
                    ySize = 10;
                    currentBuildingMesh.sharedMesh = Resources.Load<MeshFilter>("Meshes/Apartment").sharedMesh;
                    //currentBoxCollider.size = new Vector3(xSize, ySize, zSize) * ConstructionController.PointSize;//вот тут x,y,z перемножать на размер одного поинта
                    break;
            }
        }
    }

}