using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;

public class TransformBuilding : MonoBehaviour
{
    public bool isBuildingSelected, isTouchOverBuilding;
    BoxCollider boxCollider;
    GridLayout gridLayout;
    float yBuildingPos;


    void Start()
    {
        gridLayout = GameObject.Find("Construction Controller").GetComponent<GridLayout>();

        Vector3Int cellPos = gridLayout.LocalToCell(new Vector3(gameObject.transform.position.x, gameObject.transform.Find("3D Model").transform.localScale.y / 2, gameObject.transform.position.z));
        gameObject.transform.position = gridLayout.CellToLocal(cellPos);

        SetValues();
    }

    public void SetValues()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        yBuildingPos = GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.y / 2;
    }

    void Update()
    {
        Transform();
    }

    private void Transform()
    {
        if (Input2.TouchCount == 0 && Input2.OldTouchCount == 1) boxCollider.enabled = true;
        if (Input2.TouchCount != 1) { return; }
        if (!RayCaster.isHit) return;

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
            boxCollider.enabled = true;
            return;
        }

        if (isTouchOverBuilding && isBuildingSelected && Input2.OldTouchCount == 1 && RayCaster.hit.collider.gameObject.tag == "Land")
        {
            if (SceneStateController.CurrentSceneState == SceneState.Normal) SceneStateController.ChangeSceneState(SceneState.NormalBuildingSelected);

            Vector3Int cellPos = gridLayout.LocalToCell(new Vector3(RayCaster.hit.point.x, yBuildingPos, RayCaster.hit.point.z));

            gameObject.transform.position = gridLayout.CellToLocal(cellPos);
        }
    }
}
