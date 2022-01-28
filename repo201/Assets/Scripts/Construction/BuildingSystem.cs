using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;
using UnityEngine.EventSystems;


namespace ConstructionBehaviour
{
    public class BuildingSystem : MonoBehaviour
    {
        public bool isBuildingSelected, isTouchOverBuilding;


        //HashSet<Vector2> buildingPool;

        GridLayout gridLayout;


        Building currentConstruction;


        void Start()
        {
            gridLayout = GameObject.Find("Construction Controller").GetComponent<GridLayout>();
            SceneStateController.AddHandler(Clean);
        }

        float yBuildingPos;
        public void InstantiateConstruction(ConstructionType constructionType)
        {
            if (currentConstruction == null)
            {
                currentConstruction = new Building(constructionType, gridLayout);
            }
            else
            {
                currentConstruction.ChangeType(constructionType);
            }
            yBuildingPos = GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.y / 2;

        }
        public void CanselInstantiateConstruction()
        {
            GameObject.Find("GUI Controller").GetComponent<GUITransform>().StopTransformGUI();
            currentConstruction.Destroy();
            currentConstruction = null;
        }
        public void AddConstruction()
        {
            currentConstruction.name = "Building Prefab" + gridLayout.LocalToCell(currentConstruction.transform.position);
            currentConstruction.Build();

            currentConstruction = null;
        }



        void Update()
        {
            TransformBuilding();
        }


        void TransformBuilding()
        {
            if(Input2.TouchCount == 0 && Input2.OldTouchCount == 1 && currentConstruction!= null) currentConstruction.boxCollider.enabled = true;
            if (Input2.TouchCount != 1) { return; }
            if (currentConstruction == null || !RayCaster.isHit) return;


            if (Input2.OldTouchCount == 0 && Input2.TouchCount == 1 && isBuildingSelected && RayCaster.hit.collider.gameObject.name == "Building Prefab")
            {
                isTouchOverBuilding = true;
                RayCaster.hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                return;
            }
            else if (Input2.OldTouchCount == 0 && Input2.TouchCount == 1 && RayCaster.hit.collider.gameObject.name != "Building Prefab")
            {
                GameObject.Find("Building Prefab").GetComponent<Outline>().enabled = false;
                isTouchOverBuilding = false;
                isBuildingSelected = false;
                SceneStateController.ChangeSceneState(SceneState.Normal);
                currentConstruction.boxCollider.enabled = true;
                return;
            }

            if (isTouchOverBuilding && isBuildingSelected && Input2.OldTouchCount == 1 && RayCaster.hit.collider.gameObject.tag == "Land")
            {
                if (SceneStateController.CurrentSceneState == SceneState.Normal) SceneStateController.ChangeSceneState(SceneState.NormalBuildingSelected);

                Vector3Int cellPos = gridLayout.LocalToCell(new Vector3(RayCaster.hit.point.x, yBuildingPos, RayCaster.hit.point.z));

                currentConstruction.transform.position = gridLayout.CellToLocal(cellPos);
            }
        }

        void Clean()
        {
            if(SceneStateController.CurrentSceneState == SceneState.External)
            {
                currentConstruction?.Destroy();
                currentConstruction = null;
            }
        }
    }

}