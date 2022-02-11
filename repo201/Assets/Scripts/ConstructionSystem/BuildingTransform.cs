using SceneBehavior;
using UnityEngine;
using RayBehaviour;

namespace ConstructionBehaviour
{
    public class BuildingTransform : MonoBehaviour
    {
        private bool isTouchOverBuilding;
        private GridLayout gridLayout;

        //delegate void TransformState(SceneState state);
        //static event TransformState Notify;

        private GameObject cameraLogic;

        void Start()
        {
            gridLayout = GameObject.Find("Grid Controller").GetComponent<GridLayout>();
            cameraLogic = GameObject.Find("Camera Logic");
            //Notify += Camera.main.gameObject.GetComponent<CameraLogic>().ForcedChangeCamLogic;
        }

        void Update()
        {
            TransformLogic();
        }

        private void TransformLogic()
        {
            if (Input2.TouchCount != 1 && !cameraLogic.activeSelf) {/*Notify.Invoke(SceneState.Normal);*/ cameraLogic.SetActive(true); return; }
            if (!RayCaster.isHit) { return; }

            if (Input2.OldTouchCount == 0 && Input2.TouchCount == 1 && RayCaster.hit.collider.gameObject.tag == "Building Preview")
            {
                cameraLogic.SetActive(false);
                //Notify.Invoke(SceneState.Default);
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
                Vector3 value = gridLayout.CellToLocal(gridLayout.LocalToCell(new Vector3(RayCaster.hit.point.x, gameObject.transform.localScale.y / 2, RayCaster.hit.point.z)));
                gameObject.transform.position = value;
            }
        }

        ~BuildingTransform()
        {
            //cameraLogic.SetActive(true);
            //Notify -= Camera.main.gameObject.GetComponent<CameraLogic>().ForcedChangeCamLogic;
        }
    }

}
