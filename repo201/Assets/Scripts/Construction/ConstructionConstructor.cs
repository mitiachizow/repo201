using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConstructionBehaviour
{
    //Сделать класс синглтоном?????или статичным
    public class ConstructionConstructor
    {

        public readonly GameObject obj;
        MeshFilter currentBuildingMesh;
        BoxCollider currentBoxCollider;

        public ConstructionConstructor(string prefabToLoad)
        {
            //float rad = (Camera.main.transform.rotation.eulerAngles.y * Mathf.PI) / 180f;

            //float speed = 2f;

            //float zDeltaPosition = ((GameObject.Find("Camera Anchor").transform.position * Mathf.Cos(rad)) - (xLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;
            //float xDeltaPosition = ((xLocalPosition * Mathf.Cos(rad)) + (zLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;

            obj =  MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Building Prefab"),GameObject.Find("Camera Anchor").transform.position,new Quaternion());

            currentBuildingMesh = obj.transform.Find("3D Model").GetComponent<MeshFilter>();
            currentBuildingMesh.sharedMesh = Resources.Load<MeshFilter>("Meshes/" + prefabToLoad).sharedMesh;

            currentBoxCollider = obj.GetComponent<BoxCollider>();
            currentBoxCollider.size = obj.transform.Find("3D Model").transform.localScale;

            obj.name = "Building Prefab";
        }

        public void ChangeType(string objectType)
        {
            switch (objectType)
            {
                case "Factory":
                    currentBuildingMesh.sharedMesh = Resources.Load<MeshFilter>("Meshes/Factory").sharedMesh;
                    break;
                case "Apartment":
                    currentBuildingMesh.sharedMesh = Resources.Load<MeshFilter>("Meshes/Apartment").sharedMesh;
                    break;
            }
        }
    }

}