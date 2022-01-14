using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConstructionBehaviour
{
    public class ConstructionController : MonoBehaviour
    {
        public static int PointSize = 2;
        public delegate void BuildingMessage(GameObject building);

        event BuildingMessage AddBuilding;
        event BuildingMessage RemoveBuilding;

        public Construction currentConstruction;

        public void InstantiateConstruction(string constructionType)
        {
            if(currentConstruction==null)
            {
                currentConstruction = new Construction(constructionType);
            }
            else
            {
                //Debug.Log(currentConstruction.currentConstruction.name);                
                currentConstruction.ChangeType(constructionType);
                GameObject.Find(currentConstruction.currentConstruction.name).transform.Find("UI").GetComponent<ConstructionUIBehaviour>().FindRadius();

            }
        }

        HashSet<Vector2Int> fastPool;

        void Start()
        {

            fastPool = new HashSet<Vector2Int>();
        }

        public void AddNewConstruction(/*GameObject gameObject*/)
        {
            //Construction construction = currentConstruction;
            //Debug.Log(construction.pivot);

            AddFastPool();

            void AddFastPool()
            {
                fastPool.Add(currentConstruction.pivot);
                //Debug.Log(construction.pivot);
            }

            AddBuilding?.Invoke(gameObject);
            currentConstruction = null;
        }

        public void RemoveConstruction(/*GameObject gameObject*/)
        {
            float xSize = currentConstruction.xSize;
            float ySize = currentConstruction.ySize;
            Vector2Int pivot  = currentConstruction.pivot;

            for (int i = pivot.x; i < xSize + pivot.x; i--)
            {
                for (int j = pivot.y; j < ySize + pivot.y; j--)
                {
                    fastPool.Remove(new Vector2Int(i,j));
                }
            }

            //RemoveBuilding.Invoke(currentConstruction.currentConstruction);
            //currentConstruction = null;
        }

        public bool CheckPool(Vector2Int pivot,int xSize, int ySize)
        {
            for (int i = pivot.x; i < xSize + pivot.x; i++)
            {
                for (int j = pivot.y; j < ySize + pivot.y; j++)
                {
                    if (fastPool.Contains(new Vector2Int(i, j)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}