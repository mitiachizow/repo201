using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SceneBehavior;
using UnityEngine.EventSystems;


namespace ConstructionBehaviour
{
    public class GridBuildingSystem : MonoBehaviour //Rename to BuildingSystem
    {
        private Tilemap tileStatusMap;
        private Material tileStatusMapMaterial;
        private float tileStatusMapMaterialAlpha;
        private Dictionary<TileType, TileBase> tileStatusPool = new Dictionary<TileType, TileBase>();
        private Dictionary<string, TileBase> tileConstructionPool = new Dictionary<string, TileBase>();
        public bool isBuildingSelected,isTouchOverBuilding;



        SceneStateController stateController;

        //private TileBase[] oldBounds;
        //int oldTouchCount, touchCount;

        GridLayout gridLayout;


        ConstructionConstructor currentConstruction;


        void Start()
        {
            gridLayout = GameObject.Find("Construction Layer").GetComponent<GridLayout>();

            stateController = GameObject.Find("Scene State Controller").GetComponent<SceneStateController>();
            tileStatusMap = GameObject.Find("Tile Status Map").GetComponent<Tilemap>();

            tileStatusPool.Add(TileType.Empty, Resources.Load<TileBase>("Tiles/Empty"));
            tileStatusPool.Add(TileType.Available, Resources.Load<TileBase>("Tiles/Available"));
            tileStatusPool.Add(TileType.NotAvailable, Resources.Load<TileBase>("Tiles/NotAvailable"));

            SetTileStatusMap(new BoundsInt(xMin: 30, yMin: 10, zMin: 1, sizeX: 20, sizeY: 11, sizeZ: 1), tileStatusPool[TileType.Empty], ref tileStatusMap);
        }

        private void SetTileStatusMap(BoundsInt bounds, TileBase tileBase, ref Tilemap map)
        {
            TileBase[] tileArray = new TileBase[bounds.size.x * bounds.size.y * bounds.size.z];

            for (int index = 0; index < tileArray.Length; index++)
            {
                tileArray[index] = tileBase;
            }
            //oldBounds = tileStatusMap.GetTilesBlock(bounds);
            map.SetTilesBlock(bounds, tileArray);
        }

        //private void EraiseTileStatusMap()
        //{
        //    SetTileStatusMap(oldBounds);
        //}

        public void CanselInstantiateConstruction()
        {
            //currentConstruction.Destroy();
            //currentConstruction.SetName();
            currentConstruction = null;
        }

        public void InstantiateConstruction(string constructionType)
        {
            if (currentConstruction == null)
            {
                currentConstruction = new ConstructionConstructor(constructionType);
            }
            else
            {
                currentConstruction.ChangeType(constructionType);
                //GameObject.Find(currentConstruction.currentConstruction.name).transform.Find("UI").GetComponent<GUIController>().FindRadius();
            }
        }

        public void AddNewConstruction()
        {
            currentConstruction.obj.name = "Building Prefab" + gridLayout.LocalToCell(currentConstruction.obj.transform.position);//сделать геймобджект паблик ридонли?
            currentConstruction = null;
        }

        Vector2 intermediateBuildingPosition;


        //Добавить обработку на угол
        void Update()
        {
            //oldTouchCount = touchCount;
            //touchCount = Multiplatform.GetTouchCount;

            TransformBuilding();
        }

        void TransformBuilding()
        {
            if (Input2.TouchCount != 1) { isTouchOverBuilding = false; return; }
            if (Input2.IsPointerOverUI()) return;

            if (currentConstruction != null && RayCaster.isHit)
            {

                if (Input2.OldTouchCount == 0 && Input2.TouchCount == 1 && RayCaster.hit.collider.gameObject.name == "Building Prefab")
                { isTouchOverBuilding = true; intermediateBuildingPosition = new Vector2(GameObject.Find("Building Prefab").transform.position.x, GameObject.Find("Building Prefab").transform.position.z); }

                if (isBuildingSelected && Input2.OldTouchCount == 1 && Input2.TouchCount == 0) { intermediateBuildingPosition = new Vector2(GameObject.Find("Building Prefab").transform.position.x, GameObject.Find("Building Prefab").transform.position.z); }
                if (isTouchOverBuilding && isBuildingSelected)
                {
                    if (SceneStateController.CurrentSceneState == SceneState.Normal) stateController.ChangeSceneState("BuildingMovement");
                    //Работает только для андроида, нужно написать свою реализацию Input.GetTouch(0).deltaPosition.x для мыши


                    float zLocalPosition = Input.GetTouch(0).deltaPosition.y;
                    float xLocalPosition = Input.GetTouch(0).deltaPosition.x;

                    float rad = (Camera.main.transform.rotation.eulerAngles.y * Mathf.PI) / 180f;

                    float speed = 2f;

                    float zDeltaPosition = ((zLocalPosition * Mathf.Cos(rad)) - (xLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;
                    float xDeltaPosition = ((xLocalPosition * Mathf.Cos(rad)) + (zLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;


                    intermediateBuildingPosition += new Vector2(xDeltaPosition, zDeltaPosition);

                    Vector3Int cellPos = gridLayout.LocalToCell(new Vector3(intermediateBuildingPosition.x, 2f, intermediateBuildingPosition.y));

                    currentConstruction.obj.transform.position = gridLayout.CellToLocal(cellPos);
                }
                else if (SceneStateController.CurrentSceneState == SceneState.BuildingMovement)
                {
                    stateController.ChangeSceneState("Normal");
                    isBuildingSelected = false;
                    isTouchOverBuilding = false;
                }
            }
        }



        /// <summary>
        /// Типы тайлов, характеризуют состояния сетки строительства
        /// </summary>
        public enum TileType
        {
            Empty,
            Available,
            NotAvailable
        }
    }

}


//tileStatusMap.size = new Vector3Int(1000, 1000, 1);
//tileStatusMap.

//tileStatusMap.FloodFill(new Vector3Int(0, 0, 1), tilePool[TyleType.Empty]);
//tileMap = GameObject.Find("Tile Map");
//tileMap.GetComponent<Renderer>().
//tileStatusMapMaterial.color = new Color(a: 0.4f, g: 1f, b: 1f, r: 1f);
//oldBounds
//EraiseTileStatusMap();
//EraiseTileStatusMap();
//EraiseTileStatusMap();
//EraiseTileStatusMap();
//EraiseTileStatusMap();
//EraiseTileStatusMap();
//SetTileStatusMap(new BoundsInt(xMin: -20, yMin: 12, zMin: 1, sizeX: 10, sizeY: 13, sizeZ: 1));

//tileStatusMap.cellBounds = new BoundsInt(xMin:-10,yMin:1,zMin:-10,sizeX:20,sizeY:20,sizeZ:1);

//tileStatusMapMaterialAlpha = Mathf.Clamp01(tileStatusMapMaterialAlpha);
//tileStatusMapMaterial.color = new Color(a: tileStatusMapMaterialAlpha, g: 1f, b: 1f, r: 1f);