using UnityEngine;
using UIModules;
using SceneBehavior;

namespace ConstructionBehaviour
{
    public class Apartment : Construction
    {
        public Apartment(ConstructionPrefab prefab, ConstructionLVL сonstructionLvl, UnityEngine.Transform parent, Vector3 positon, GridLayout grid) : base(prefab, сonstructionLvl, parent, positon, grid) { }

        public override void ShowInfo()
        {
            switch(SceneStateController.CurrentSceneState)
            {
                case SceneState.Building:
                    GameObject.Find("Canvas Controller").GetComponent<CanvasController>().ForceChangeCanvasParts(infoPlane: true, addConstruction: true);
                    GameObject.Find("Info Plane").GetComponent<InfoPlane>().SetInfo("Apartment");
                    break;
                case SceneState.Normal:
                    GameObject.Find("Canvas Controller").GetComponent<CanvasController>().ForceChangeCanvasParts(infoPlane: true);
                    GameObject.Find("Info Plane").GetComponent<InfoPlane>().SetInfo("Apartment");
                    break;
            }
        }

        public override void Upgrade()
        {
            throw new System.NotImplementedException();
        }

        ~Apartment()
        {
            Debug.Log("Apartment is dead");
        }

    }

}

