using UnityEngine;
using SceneBehavior;

namespace ConstructionBehaviour
{
    public abstract class Construction : IInfo
    {
        protected GameObject gameObject;
        //protected Vector3 Size;
        protected ConstructionLVL сonstructionLVL;

        public string Tag
        {
            get
            {
                return gameObject.tag;
            }
            set
            {
                gameObject.tag = value;
            }
        }
        public Vector3 Position 
        {
            get
            {
                return gameObject.transform.position;
            }
        }
        public string Name
        {
            get
            {
                return gameObject.name;
            }
        }
        public abstract string GetInfo();
        public abstract void Upgrade();

        public void Build()
        {
            gameObject.tag = "Building";
            gameObject.name = gameObject.name + gameObject.transform.position; //поменять
        }

        protected Construction(ConstructionPrefab prefab, ConstructionLVL сonstructionLvl, Transform parent, Vector3 positon, GridLayout grid)
        {
            //this.Size = prefab.Size;
            this.сonstructionLVL = сonstructionLvl;

            gameObject = new GameObject();

            gameObject.AddComponent<MeshRenderer>();

            gameObject.AddComponent<MeshFilter>();
            gameObject.GetComponent<MeshFilter>().sharedMesh = prefab.Mesh;

            gameObject.AddComponent<BoxCollider>();

            gameObject.transform.position = grid.CellToLocal(grid.LocalToCell(new Vector3(positon.x, prefab.Size.y / 2, positon.y)));
            gameObject.transform.parent = parent;
            gameObject.tag = "Building Preview";
            gameObject.name = prefab.Name;
            gameObject.transform.localScale = prefab.Size;

            //prefab.RandomBuilding();
        }

        public void KillMe() //ЗАМЕНИТЬ НА ДЕСТРУКТОР
        {
            GameObject.Destroy(gameObject);
            Debug.Log("Construction is dead");
        }


    }

}
