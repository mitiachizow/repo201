using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;

namespace ConstructionBehaviour
{
    public class Building
    {
        private MeshFilter buildingMesh;
        private GameObject obj;

        public BoxCollider boxCollider { get; set; }
        public string name
        {
            get
            {
                return obj.name;
            }
            set
            {
                obj.name = value;
            }
        }
        public Transform transform;

        public Building(ConstructionType prefabToLoad, GridLayout gridLayout)
        {
            obj =  MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Building Prefab"),GameObject.Find("Camera Anchor").transform.position,new Quaternion(), GameObject.Find("Construction Pool").transform);
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.Find("3D Model").transform.localScale.y / 2, obj.transform.position.z);
            //new Vector3(GameObject.Find("Camera Anchor").transform.position.x,GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.y/2, GameObject.Find("Camera Anchor").transform.position.z)


            boxCollider = obj.GetComponent<BoxCollider>();
            transform = obj.GetComponent<Transform>();
            buildingMesh = obj.transform.Find("3D Model").GetComponent<MeshFilter>();
            buildingMesh.sharedMesh = Resources.Load<MeshFilter>("Meshes/" + prefabToLoad).sharedMesh;

            boxCollider.size = obj.transform.Find("3D Model").transform.localScale;

            Vector3Int cellPos = gridLayout.LocalToCell(transform.position);
            transform.position = gridLayout.CellToLocal(cellPos);

            name = obj.name = "Building Prefab";
        }

        public void ChangeType(ConstructionType objectType)
        {
            buildingMesh.sharedMesh = Resources.Load<MeshFilter>($"Meshes/{objectType}".ToString()).sharedMesh;
            GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().isBuildingSelected = true;
            GameObject.Find("Building Prefab").GetComponent<Outline>().enabled = true;
        }

        public void Destroy()
        {
            //GameObject.Find("GUI Controller").GetComponent<GUITransform>().StopTransformGUI();
            GameObject.Destroy(obj);
        }
        
        public void Build()
        {
            GameObject.Destroy(obj.GetComponent<Outline>());
            //Отсюда передаем всякие стат данные в глобал предиктор
        }

    }

}