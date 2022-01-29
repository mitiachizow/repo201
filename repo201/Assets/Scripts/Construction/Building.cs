using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;

namespace ConstructionBehaviour
{
    public class Building
    {
        private GameObject obj;

        public Building(ConstructionType prefabToLoad)
        {
            obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Building Prefab"), GameObject.Find("Camera Anchor").transform.position, new Quaternion(), GameObject.Find("Construction Pool").transform);

            obj.transform.Find("3D Model").GetComponent<MeshFilter>().sharedMesh = Resources.Load<MeshFilter>("Meshes/" + prefabToLoad).sharedMesh;
            obj.GetComponent<BoxCollider>().size = obj.transform.Find("3D Model").transform.localScale;
            obj.name = "Building Prefab";
        }

        public void ChangeType(ConstructionType objectType)
        {
            obj.transform.Find("3D Model").GetComponent<MeshFilter>().sharedMesh = Resources.Load<MeshFilter>($"Meshes/{objectType}".ToString()).sharedMesh;
            GameObject.Find("Building Prefab").GetComponent<Outline>().enabled = true;
        }

        public void Destroy()
        {
            GameObject.Destroy(obj);
        }

        public void Build()
        {
            obj.name = "Building Prefab" + GameObject.Find("Construction Controller").GetComponent<GridLayout>().LocalToCell(obj.transform.position);
            GameObject.Destroy(obj.GetComponent<Outline>());
            //Отсюда передаем всякие стат данные в глобал предиктор
        }

        private (int, int) InitParams(ConstructionType type)
        {
            (int val1, int val2) values;
                switch(type)
                {
                default:
                    case ConstructionType.Apartment:
                    values = (val1:12, val2: 222);
                        break;

                }
                return values;
        }


    }

}