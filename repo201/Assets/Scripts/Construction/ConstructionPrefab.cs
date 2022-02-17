using UnityEngine;

namespace ConstructionBehaviour
{
    [CreateAssetMenu(fileName = "Construction", menuName = "Construction")]
    public class ConstructionPrefab : ScriptableObject
    {
        [SerializeField]
        private ConstructionType constructionType;
        [SerializeField]
        private Texture2D[] previewImage;
        [SerializeField]
        private Mesh[] mesh;
        [SerializeField]
        private Vector3Int[] size;

        private int randomNumber = default;

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
            get
            {
                randomNumber = Random.Range(0, mesh.Length);
                return mesh[randomNumber];
            }
        }

        public string Name
        {
            get
            {
                return constructionType.ToString();
                //switch(constructionType)
                //{
                //    case ConstructionType.Apartment:
                //        return;
                //    case ConstructionType.Factory:
                //        return;
                //    case ConstructionType.Road:
                //        return;
                //    default:
                //        return;
                //}
            }
        }


    }
}


