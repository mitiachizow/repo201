using UnityEngine;
using System.Collections.Generic;
using UIModules;

namespace ConstructionBehaviour
{
    public class ConstructionSystem : MonoBehaviour
    {
        [SerializeField] private GameObject infoPlane;
        [SerializeField] private ConstructionConstructor constructor;
        [SerializeField] private CanvasController canvasController;


        private Dictionary<Vector3, Construction> constructionPool = new Dictionary<Vector3, Construction>();

        private Construction currentConstruction;
        private GameObject selectedConstruction;

        public void CreateConstruction()
        {
            constructionPool.Add(currentConstruction.Position,currentConstruction);
            currentConstruction.Build();
            currentConstruction = null;
        }

        public void SpawnConstruction(ConstructionPrefab prefab)
        {
            if (currentConstruction?.Name == prefab.Name) return; //Если уже инициализирован объект с именем А и юзер пытается еще раз создать объект с именем А, инициализация не произойдет
            else if (currentConstruction != null) { currentConstruction.KillMe(); currentConstruction = null; }

            currentConstruction = constructor.Create(prefab, ConstructionLVL.LVL1);

            infoPlane.GetComponent<InfoTab>().SetInfo(currentConstruction);
            infoPlane.SetActive(true);

            canvasController.ForceChangeCanvasState(sceneStateSelector: false);
        }

        public void CanselCreateConstruction()
        {
            currentConstruction.KillMe();
            currentConstruction = null;
        }

        public void UpgradeConstruction()
        {
            gameObject.GetComponent<Construction>().Upgrade();
        }

        public void DeCreateConstruction()
        {
            constructionPool.Remove(selectedConstruction.transform.position);
        }

        public void AddSelectedConstruction(GameObject selected)
        {
            selectedConstruction = selected;
        }

        public Construction GetConstrution(Vector3 key)
        {
            return constructionPool[key];
        }
    }

}

