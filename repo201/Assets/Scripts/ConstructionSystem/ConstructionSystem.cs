using UnityEngine;
using System.Collections.Generic;

namespace ConstructionBehaviour
{
    public class ConstructionSystem : MonoBehaviour
    {
        //[SerializeField]
        //private List<Construction> constructionsPool = new List<Construction>();
        //public ConstructionPrefab prefab;
        private Dictionary<Vector3, Construction> constructionPool = new Dictionary<Vector3, Construction>();

        private Construction currentConstruction;
        private GameObject selectedConstruction;
        //private GameObject currentGameObject;

        public void CreateConstruction()
        {
            constructionPool.Add(currentConstruction.Pivot,currentConstruction);
            currentConstruction.Build();
            currentConstruction = null;
        }

        public void InitialiseConstruction(ConstructionPrefab prefab)
        {
            if (currentConstruction != null && currentConstruction.Name == prefab.Name) return; //Если уже инициализирован объект с именем А и юзер пытается еще раз создать объект с именем А, инициализация не произойдет
            else if (currentConstruction != null) { currentConstruction.KillMe(); currentConstruction = null; }
            Init();
            currentConstruction.ShowInfo();

            void Init()
            {
                switch (prefab.ConstructionType)
                {
                    case ConstructionType.Apartment:
                        currentConstruction = new Apartment(prefab, ConstructionLVL.LVL1, GameObject.Find("Construction Pool").transform, GameObject.Find("Camera Anchor").transform.position, GameObject.Find("Grid Controller").GetComponent<GridLayout>());
                        break;
                    case ConstructionType.Factory:
                        //currentConstruction = new Apartment(prefab, ConstructionLVL.LVL1, GameObject.Find("Construction Pool").transform, GameObject.Find("Camera Anchor").transform.position, GameObject.Find("Grid Controller").GetComponent<GridLayout>());
                        break;
                }
            }
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
    }

}

