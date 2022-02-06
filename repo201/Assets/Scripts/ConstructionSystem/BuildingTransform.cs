using SceneBehavior;
using UnityEngine;
using CameraBehavior;

public class BuildingTransform : MonoBehaviour
{
    public bool isTouchOverBuilding;
    GridLayout gridLayout;

    delegate void TransformState(SceneState state);
    static event TransformState Notify;


    void Start()
    {
        gridLayout = GameObject.Find("Grid Controller").GetComponent<GridLayout>();
        Notify += Camera.main.gameObject.GetComponent<CameraLogic>().ForcedChangeCamLogic;
    }

    void Update()
    {
        TransformLogic();
    }

    private void TransformLogic()
    {
        if (Input2.TouchCount != 1) {Notify.Invoke(SceneState.Normal);  return; }
        if (!RayCaster.isHit) { return; }

        if (Input2.OldTouchCount == 0 && Input2.TouchCount == 1 && RayCaster.hit.collider.gameObject.tag == "Building Preview")
        {
            Notify.Invoke(SceneState.Default);
            isTouchOverBuilding = true;
            return;
        }
        else if (Input2.OldTouchCount == 0 && Input2.TouchCount == 1 && RayCaster.hit.collider.gameObject.tag != "Building Preview")
        {
            isTouchOverBuilding = false;
            return;
        }

        if (isTouchOverBuilding && Input2.OldTouchCount == 1 && RayCaster.hit.collider.gameObject.tag == "Land")
        {
            Vector3 value = gridLayout.CellToLocal(gridLayout.LocalToCell(new Vector3(RayCaster.hit.point.x, 16f, RayCaster.hit.point.z)));
            gameObject.transform.position = value;
        }
    }

    ~BuildingTransform()
    {
        Notify -= Camera.main.gameObject.GetComponent<CameraLogic>().ForcedChangeCamLogic;
    }
}
