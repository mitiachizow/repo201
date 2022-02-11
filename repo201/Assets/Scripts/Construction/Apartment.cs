using UnityEngine;
using UIModules;

namespace ConstructionBehaviour
{
    public class Apartment : Construction
    {
        public Apartment(ConstructionPrefab prefab, ConstructionLVL сonstructionLvl, UnityEngine.Transform parent, Vector3 positon, GridLayout grid) : base(prefab, сonstructionLvl, parent, positon, grid) { }

        public override void ShowInfo()
        {
            GameObject.Find("Canvas Controller").GetComponent<CanvasController>().ForceChangeCanvasParts(infoPlane: true, addConstruction: true);
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

