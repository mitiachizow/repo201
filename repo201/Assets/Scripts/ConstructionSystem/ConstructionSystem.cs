using UnityEngine;

namespace ConstructionBehaviour
{
    public class ConstructionSystem : MonoBehaviour
    {
        //[SerializeField]
        //private List<Construction> constructionsPool = new List<Construction>();
        //public ConstructionPrefab prefab;
        //private Dictionary<Vector3Int, Construction> constructionPool = new Dictionary<Vector3Int, Construction>();

        private Construction currentConstruction;
        //private GameObject currentGameObject;

        private void Start()
        {
            //currentConstruction = new Apartment(prefab, ConstructionLVL.LVL1, GameObject.Find("Construction Pool").transform, GameObject.Find("Camera Anchor").transform.position, GameObject.Find("Grid Controller").GetComponent<GridLayout>());
        }

        public void CreateConstruction()
        {
            //constructionPool.Add(currentConstruction.Pivot,currentConstruction);
            currentConstruction = null;
        }

        public void InitialiseConstruction(ConstructionPrefab prefab)
        {
            if (currentConstruction != null) { currentConstruction.KillMe(); currentConstruction = null; }
            else if (currentConstruction != null && currentConstruction.Name == prefab.Name) return ; //Если два одинаковых здания, нет нужны каждый раз спавнить его!!!!!!!!!!!!!!!!!!!!!!!!!!! // тут по какой то причине не работает проверка
            Init();

            void Init()
            {
                switch (prefab.ConstructionType)
                {
                    case ConstructionType.Apartment:
                        currentConstruction = new Apartment(prefab, ConstructionLVL.LVL1, GameObject.Find("Construction Pool").transform, GameObject.Find("Camera Anchor").transform.position, GameObject.Find("Grid Controller").GetComponent<GridLayout>());
                        break;
                    case ConstructionType.Factory:
                        //currentConstruction = new Factory(prefab, ConstructionLVL.LVL1, GameObject.Find("Construction Pool").transform, GameObject.Find("Camera Anchor").transform.position, GameObject.Find("Grid Controller").GetComponent<GridLayout>());
                        break;
                }

                currentConstruction = new Apartment(prefab, ConstructionLVL.LVL1, GameObject.Find("Construction Pool").transform, GameObject.Find("Camera Anchor").transform.position, GameObject.Find("Grid Controller").GetComponent<GridLayout>());
                currentConstruction.ShowInfo();
                

            }
        }

        public void DeSpawnConstruction()
        {

        }
    }

}

