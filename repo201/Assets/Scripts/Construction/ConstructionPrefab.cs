using UnityEngine;

namespace ConstructionBehaviour
{
    [CreateAssetMenu(fileName = "Construction", menuName = "Construction")]
    public class ConstructionPrefab : ScriptableObject
    {
        [SerializeField]
        private Vector3Int[] size;
        [SerializeField]
        private ConstructionType constructionType;
        [SerializeField]
        private Mesh[] mesh;
        [SerializeField]
        private new string name;
        [SerializeField]
        private Texture2D previewTexture;

        //public void Awake()
        //{
        //    randomNumber = GetRandomNubmer();
        //    Debug.Log("start init well");
        //    //Avake очень плохо работает в этом месте, поменять логиику вызова рандома
        //}

        public string Name
        {
            get { return name; }
        }

        public Vector3Int Size
        {
            get { return size[randomNumber]; }
        }

        public ConstructionType ConstructionType
        {
            get { return constructionType; }
        }

        public Mesh Mesh
        {
            get { return mesh[randomNumber]; }
        }

        private int randomNumber = default;
        public void RandomBuilding()
        {
            randomNumber = Random.Range(0, mesh.Length);
        }

        //private int randomNumber;
        //private int GetRandomNubmer()
        //{
        //    return 0/*Random.Range(0, mesh.Length)*/;
        //}
    }
}


