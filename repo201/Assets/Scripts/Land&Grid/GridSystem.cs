using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;

namespace ConstructionBehaviour
{
    public class GridSystem : MonoBehaviour
    {
        public Dictionary<Vector2Int, GridCellStatus> gridPool { get; private set; }
        //static GridLayout gridLayout;

        private void Start()
        {
            //gridLayout = gameObject.GetComponent<GridLayout>();
            gridPool = new Dictionary<Vector2Int, GridCellStatus>();
        }

        public void FillGrid(Vector2Int pivotPos)
        {
            for (int bottomXPos = pivotPos.x - SceneParams.cellSize/2 -  SceneParams.landLenght / 2; bottomXPos < pivotPos.x + SceneParams.landLenght / 2 - SceneParams.cellSize / 2; bottomXPos += SceneParams.cellSize)
            {
                for (int bottomYPos = pivotPos.y -SceneParams.cellSize / 2 - SceneParams.landLenght / 2; bottomYPos < pivotPos.y + SceneParams.landLenght / 2 - SceneParams.cellSize / 2; bottomYPos += SceneParams.cellSize)
                {
                    gridPool.Add(new Vector2Int(bottomXPos, bottomYPos),GridCellStatus.Empty);
                }
            }
        }

        //public static Vector3Int VectorToGrid(Vector3 vector)
        //{
        //    return gridLayout.cell
        //}

        public enum GridCellStatus
        {
            Empty,
            Busy
        }
    }

}

