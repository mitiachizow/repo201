using UnityEngine;
using UIModules;
using SceneBehavior;

namespace ConstructionBehaviour
{
    public class Apartment : Construction
    {
        public Apartment(ConstructionPrefab prefab, ConstructionLVL сonstructionLvl, Transform parent, Vector3 position, GridLayout grid) : base(prefab, сonstructionLvl, parent, position, grid) { }



        //public override void GetInfo()
        //{
        //    switch(SceneStateController.CurrentSceneState)
        //    {
        //        case SceneState.Building:
        //            CanvasController.ForceChangeCanvasParts(infoPlane: true, addConstruction: true);
        //            InfoPlane.SetInfo("Apartment");
        //            break;
        //        case SceneState.Normal:
        //            CanvasController.ForceChangeCanvasParts(infoPlane: true);
        //            InfoPlane.SetInfo("Apartment");
        //            break;
        //    }
        //}

        public override string GetInfo()
        {
            return base.Name;
        }

        public override void Upgrade()
        {
            //throw new System.NotImplementedException();
        }

        ~Apartment()
        {
            Debug.Log("Apartment is dead");
        }

    }

}

