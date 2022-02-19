using SceneBehavior;
using UnityEngine;
using RayBehaviour;
using CameraBehavior;

namespace ConstructionBehaviour
{
    public class BuildingTransform : MonoBehaviour
    {
        private GridLayout gridLayout;
        private GameObject cameraLogic;
        private bool isTouchOverBuilding;

        void Update() => TransformLogic();

        private void TransformLogic()
        {
            if (Input2.TouchCount != 1 && !cameraLogic.activeSelf) {cameraLogic.SetActive(true); return; }
            if (!RayCaster.IsHit) { return; }

            if (Input2.OldTouchCount == 0 && Input2.TouchCount == 1 && RayCaster.hit.collider.gameObject.tag == "Building Preview")
            {
                cameraLogic.SetActive(false);
                isTouchOverBuilding = true;
                return;
            }
            else if (Input2.OldTouchCount == 0 && Input2.TouchCount == 1 && RayCaster.hit.collider.gameObject.tag != "Building Preview")
            {
                isTouchOverBuilding = false;
                return;
            }

            if (isTouchOverBuilding && Input2.OldTouchCount == 1 && RayCaster.hit.collider.gameObject.tag == "Land") //Плохой код, изменить потом
            {
                Vector3 value = gridLayout.CellToLocal(gridLayout.LocalToCell(new Vector3(RayCaster.hit.point.x, gameObject.transform.localScale.y / 2, RayCaster.hit.point.z)));
                gameObject.transform.position = value;
            }
        }

        public void Instantiate(GridLayout gridLayout, GameObject cameraLogic)
        {
            this.gridLayout = gridLayout;
            this.cameraLogic = cameraLogic;
        }
    }

}
