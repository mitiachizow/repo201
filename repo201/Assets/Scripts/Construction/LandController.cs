using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;

namespace ConstructionBehaviour
{
    /// <summary>
    /// Временно, добавление land будет происходить случайно.
    /// Далее, можно будет выбирать, куда добавлять новый cell
    /// </summary>
    public class LandController : MonoBehaviour
    {

        Dictionary<Vector2Int, LandCellStatus> landPool;

        public void Start()
        {

            landPool = new Dictionary<Vector2Int, LandCellStatus>();

            WriteLandGrid(new Vector2Int(0, 0));
            GameObject.Find("Grid Controller").GetComponent<GridController>().FillGrid(new Vector2Int(0, 0));
        }

        public void AddCell()
        {
            Vector2Int cellToWrite = SelectCell();
            WriteLandGrid(cellToWrite);
            //Instantiate(Resources.Load<GameObject>("Prefabs/Land Prefab"),new Vector3(cellToWrite.x,-25,cellToWrite.y), new Quaternion(), GameObject.Find("Land Pool").transform);
            GameObject.Find("Grid Controller").GetComponent<GridController>().FillGrid(cellToWrite);
        }

        private void WriteLandGrid(Vector2Int pos)
        {
            LandCellStatus value;

            landPool[pos] = LandCellStatus.NotAvailable;

            if (landPool.TryGetValue(new Vector2Int(pos.x, pos.y + SceneParams.landLenght), out value));
            else landPool.Add(new Vector2Int(pos.x, pos.y + SceneParams.landLenght),LandCellStatus.Available);

            if (landPool.TryGetValue(new Vector2Int(pos.x, pos.y - SceneParams.landLenght), out value)) ;
            else landPool.Add(new Vector2Int(pos.x, pos.y - SceneParams.landLenght), LandCellStatus.Available);

            if (landPool.TryGetValue(new Vector2Int(pos.x + SceneParams.landLenght, pos.y), out value)) ;
            else landPool.Add(new Vector2Int(pos.x + SceneParams.landLenght, pos.y), LandCellStatus.Available);

            if (landPool.TryGetValue(new Vector2Int(pos.x - SceneParams.landLenght, pos.y), out value)) ;
            else landPool.Add(new Vector2Int(pos.x - SceneParams.landLenght, pos.y), LandCellStatus.Available);

            Instantiate(Resources.Load<GameObject>("Prefabs/Land Prefab"), new Vector3(pos.x, -25, pos.y), new Quaternion(), GameObject.Find("Land Pool").transform);

        }

        private Vector2Int SelectCell()
        {
            foreach (KeyValuePair<Vector2Int,LandCellStatus> item in landPool)
            {
                if (item.Value == LandCellStatus.Available) return item.Key;
            }
            throw new System.Exception("This one is impossible to get");
        }

        public enum LandCellStatus
        {
            Available,
            NotAvailable
        }

    }

}

