using UnityEngine;
using System.Collections.Generic;
using UIModules;

namespace ConstructionBehaviour
{
    public class ConstructionSystem : MonoBehaviour
    {
        //[SerializeField] private Transform constructionPoolTransform;
        //[SerializeField] private Transform cameraAnchorTransform;//вот от этого потом избавлюсь
        //[SerializeField] private GridLayout gridLayout;
        [SerializeField] private InfoPlane infoPlane;
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

            infoPlane.SetInfo(currentConstruction);
            infoPlane.gameObject.SetActive(true);
            canvasController.ForceChangeCanvasParts(sceneStateSwitcher: false) ;
            //Init();
            ////currentConstruction.GetInfo();

            //void Init()
            //{
            //    switch (prefab.ConstructionType)
            //    {
            //        case ConstructionType.Apartment:
            //            currentConstruction = new Apartment(prefab, ConstructionLVL.LVL1, constructionPoolTransform, cameraAnchorTransform.position, gridLayout);
            //            infoPlane.ShowInfo();
            //            break;
            //        case ConstructionType.Factory:
            //            //currentConstruction = new Apartment(prefab, ConstructionLVL.LVL1, GameObject.Find("Construction Pool").transform, GameObject.Find("Camera Anchor").transform.position, GameObject.Find("Grid Controller").GetComponent<GridLayout>());
            //            break;
            //    }
            //}
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

